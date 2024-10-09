using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestOrder.Data;
using TestOrder.Models;

namespace TestOrder.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var objlist = _context.SO_ORDER.ToList();
            var vmList = _mapper.Map<List<OrderViewModel>>(objlist);
            if(vmList != null && vmList.Count > 0)
            {
                foreach(var item in vmList)
                {
                    var customer = _context.COM_CUSTOMER.Where(x => x.COM_CUSTOMER_ID == item.COM_CUSTOMER_ID).FirstOrDefault();
                    item.CUSTOMER_NAME = customer != null ? customer.CUSTOMER_NAME : "";
                }
            }
            return View(vmList);
        }

        public IActionResult Create()
        {
            var ListCustomers = _context.COM_CUSTOMER.Select(x =>
            new IdNamePairVM()
            {
                Id = x.COM_CUSTOMER_ID,
                Name = x.CUSTOMER_NAME
            }).ToList();
            ViewBag.Customers = ListCustomers;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderViewModel vm)
        {
            Order order = _mapper.Map<Order>(vm);
            if (ModelState.IsValid)
            {
                _context.SO_ORDER.Add(order);
                _context.SaveChanges();
                vm.SO_ORDER_ID = order.SO_ORDER_ID;

                //add item
                if (vm.ItemList != null && vm.ItemList.Count > 0)
                {
                    vm.ItemList.ForEach(x => x.SO_ORDER_ID = vm.SO_ORDER_ID);
                    var itemList = _mapper.Map<List<Item>>(vm.ItemList);
                    _context.SO_ITEM.AddRange(itemList);
                    _context.SaveChanges();
                }
                TempData["ResultOk"] = "Record Added Successfully !";
                return RedirectToAction("Index");
            }

            return View(order);
        }

        public IActionResult Edit(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _context.SO_ORDER.Find(id);
            var vm = _mapper.Map<OrderViewModel>(obj);
            if (obj == null)
            {
                return NotFound();
            }
            var itemList = _context.SO_ITEM.Where(x=>x.SO_ORDER_ID == vm.SO_ORDER_ID).ToList();
            vm.ItemList = _mapper.Map<List<ItemViewModel>>(itemList);

            var ListCustomers = _context.COM_CUSTOMER.Select(x =>
            new IdNamePairVM()
            {
                Id = x.COM_CUSTOMER_ID,
                Name = x.CUSTOMER_NAME
            }).ToList();
            ViewBag.Customers = ListCustomers;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderViewModel vm)
        {
            Order order = _mapper.Map<Order>(vm);
            if (ModelState.IsValid)
            {
                _context.SO_ORDER.Update(order);
                _context.SaveChanges();
                //add item
                if (vm.ItemList != null && vm.ItemList.Count > 0)
                {
                    // Map
                    var listExistingItem = _context.SO_ITEM.Where(x => x.SO_ORDER_ID == vm.SO_ORDER_ID).ToList();
                    var listDeletedItem = listExistingItem;
                    if (vm.ItemList != null)
                    {
                        foreach (var updateItem in vm.ItemList)
                        {
                            var existingItem = _context.SO_ITEM.Where(x => x.SO_ITEM_ID == updateItem.SO_ITEM_ID && x.SO_ITEM_ID != 0).FirstOrDefault();
                            if (existingItem != null)
                            {
                                existingItem.ITEM_NAME = updateItem.ITEM_NAME;
                                existingItem.QUANTITY = updateItem.QUANTITY;
                                existingItem.PRICE = updateItem.PRICE;

                                listDeletedItem.Remove(existingItem);
                            }
                            else
                            {
                                _context.SO_ITEM.Add(new Item
                                {
                                    ITEM_NAME = updateItem.ITEM_NAME,
                                    QUANTITY = updateItem.QUANTITY,
                                    PRICE = updateItem.PRICE,
                                    SO_ORDER_ID = order.SO_ORDER_ID,
                                });
                            }
                        }
                    }

                    if (listDeletedItem != null && listDeletedItem.Count > 0)
                    {
                        foreach (var item in listDeletedItem)
                        {
                            _context.SO_ITEM.Remove(item);
                        }
                    }
                    _context.SaveChanges();
                }

                TempData["ResultOk"] = "Data Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View(order);
        }

        public IActionResult Delete(long? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var empfromdb = _context.SO_ORDER.Find(id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEmp(long id)
        {
            var deleterecord = _context.SO_ORDER.Find(id);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.SO_ORDER.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOk"] = "Data Deleted Successfully !";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(long id)
        {
            var deleterecord = _context.SO_ITEM.Find(id);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.SO_ITEM.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOk"] = "Data Deleted Successfully !";
            return RedirectToAction("Index");
        }
    }
}