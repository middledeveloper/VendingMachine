using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingMachine.EntityConfigurations;
using VendingMachine.Models;

namespace VendingMachine.Controllers
{
    public class AdminController : Controller
    {
        private const string AccessKey = "keyvalue";
        private const int MachineId = 1;
        private const string ProductEmptyLogo = "~/Logos/empty.png";

        public ActionResult Index(string key)
        {
            if (key == null || key.ToLower() != AccessKey)
                return RedirectToAction("Index", "Home");
            else
            {
                return View(PrepareMachineViewModel());
            }
        }

        public ActionResult RenderProductManager(int productId)
        {
            using (var ctx = new MachineContext())
            {
                return PartialView("_Product", ctx.Products.First(p => p.Id == productId));
            }
        }

        public ActionResult RenderProductForm()
        {
            return PartialView("_ProductForm", new Product() { MachineId = MachineId });
        }

        public ActionResult AddProduct(Product product)
        {
            if (product.UploadLogo == null)
                product.Image = ProductEmptyLogo;
            else
            {
                var uploadFilePath = UploadLogoServerPath(product.UploadLogo.FileName);
                if (uploadFilePath != null)
                {
                    product.UploadLogo.SaveAs(Server.MapPath(uploadFilePath));
                    product.Image = uploadFilePath;
                }
            }

            using (var ctx = new MachineContext())
            {
                ctx.Products.Add(product);
                ctx.SaveChanges();
            }

            return RedirectToAction("Index", new { key = AccessKey });
        }

        public ActionResult UpdateProduct(Product product)
        {
            string uploadFilePath = null;
            if (product.UploadLogo != null)
            {
                uploadFilePath = UploadLogoServerPath(product.UploadLogo.FileName);
                if (uploadFilePath != null)
                {
                    product.UploadLogo.SaveAs(Server.MapPath(uploadFilePath));
                }
            }

            using (var ctx = new MachineContext())
            {
                var ctxProduct = ctx.Products.First(c => c.Id == product.Id);
                ctxProduct.Name = product.Name;
                ctxProduct.Image = uploadFilePath == null ? product.Image : uploadFilePath;
                ctxProduct.Price = product.Price;
                ctxProduct.Storage = product.Storage;
                ctx.SaveChanges();
            }

            return View("Index", PrepareMachineViewModel());
        }

        public ActionResult RenderCoinManager(int coinId)
        {
            using (var ctx = new MachineContext())
            {
                return PartialView("_Coin", ctx.Coins.FirstOrDefault(c => c.Id == coinId));
            }
        }

        public ActionResult UpdateCoin(Coin coin)
        {
            using (var ctx = new MachineContext())
            {
                var ctxCoin = ctx.Coins.First(c => c.Id == coin.Id);
                ctxCoin.Blocked = coin.Blocked;
                ctxCoin.TotalCount = coin.TotalCount;
                ctx.SaveChanges();

            }

            return View("Index", PrepareMachineViewModel());
        }

        private static Machine PrepareMachineViewModel()
        {
            using (var ctx = new MachineContext())
            {
                var machine = ctx.Machine.Find(MachineId);
                machine.Products = ctx.Products.Where(p => p.MachineId == machine.Id).ToList();
                machine.Coins = ctx.Coins.Where(p => p.MachineId == machine.Id).ToList();
                machine.TotalBalance = ctx.Coins.Sum(c => c.TotalCount * c.Value);

                return machine;
            }
        }

        private static string UploadLogoServerPath(string filePath)
        {
            var uploadExtension = Path.GetExtension(filePath).ToLower();
            var validExtensions = new List<string>() { ".jpg", ".png" };
            if (validExtensions.Contains(uploadExtension))
                return "~/Logos/" + Guid.NewGuid() + uploadExtension;
            else
                return null;
        }

        public ActionResult ImportProducts(HttpPostedFileBase csv)
        {
            if (csv != null && csv.ContentLength > 0)
            {
                if (Path.GetExtension(csv.FileName).ToLower() == ".csv")
                {
                    var products = new List<Product>();
                    using (var stream = new StreamReader(csv.InputStream))
                    {
                        stream.ReadLine();
                        var row = string.Empty;
                        while ((row = stream.ReadLine()) != null)
                        {
                            var data = row.Split(';');
                            products.Add(new Product()
                            {
                                Name = data[0],
                                Image = data[1],
                                Price = Convert.ToDecimal(data[2]),
                                Storage = Convert.ToInt32(data[3]),
                                MachineId = Convert.ToInt32(data[4])
                            });
                        }
                    }

                    using (var ctx = new MachineContext())
                    {
                        ctx.Products.AddRange(products);
                        ctx.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index", new { key = AccessKey });
        }
    }
}