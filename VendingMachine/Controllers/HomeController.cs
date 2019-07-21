using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VendingMachine.EntityConfigurations;
using VendingMachine.Models;

namespace VendingMachine.Controllers
{
    public class HomeController : Controller
    {
        private const int MachineId = 1;

        public ActionResult Index()
        {
            using (var ctx = new MachineContext())
            {
                var machine = ctx.Machine.Find(MachineId);
                machine.Products = ctx.Products.Where(p => p.MachineId == machine.Id).ToList();
                machine.Coins = ctx.Coins.Where(p => p.MachineId == machine.Id).ToList();
                return View(machine);
            }
        }

        public ActionResult RenderCoinButtons(List<Coin> coins)
        {
            return PartialView("_Coins", coins);
        }

        public ActionResult AddCoin(int coinId)
        {
            using (var ctx = new MachineContext())
            {
                var machine = ctx.Machine.Find(MachineId);
                var coin = ctx.Coins.FirstOrDefault(c => c.Id == coinId && !c.Blocked);
                if (coin != null)
                {
                    machine.ClientBalance += coin.Value;
                    coin.TotalCount++;
                    ctx.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult RenderProductButtons(List<Product> products)
        {
            return PartialView("_Products", products);
        }

        public ActionResult ProvideProduct(int productId)
        {
            using (var ctx = new MachineContext())
            {
                var machine = ctx.Machine.Find(MachineId);
                var machineProduct = ctx.Products.FirstOrDefault(p => p.Id == productId);
                if (machineProduct.Storage > 0)
                {
                    machineProduct.Storage--;
                    machine.ClientBalance -= machineProduct.Price;
                    ctx.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult RenderProvideChange()
        {
            return PartialView("_ProvideChange");
        }

        public ActionResult ProvideChange()
        {
            using (var ctx = new MachineContext())
            {
                var machine = ctx.Machine.Find(MachineId);
                machine.TotalBalance = ctx.Coins.Sum(c => c.TotalCount * c.Value);
                if (machine.TotalBalance > machine.ClientBalance)
                {
                    foreach (var coin in ctx.Coins.OrderByDescending(c => c.Value).ToList())
                    {
                        var coinNeededCount = (int)(machine.ClientBalance / coin.Value);
                        if (coinNeededCount == 0)
                            continue;

                        if (coinNeededCount <= coin.TotalCount)
                        {
                            ChangeCoinWriteOff(coin.Id, coinNeededCount, ctx);
                            machine.ClientBalance %= coin.Value;
                        }
                        else
                        {
                            if (coin.TotalCount != 0)
                            {
                                while (coinNeededCount-- > 0)
                                {
                                    if (coinNeededCount == coin.TotalCount)
                                    {
                                        ChangeCoinWriteOff(coin.Id, coinNeededCount, ctx);
                                        machine.ClientBalance -= (coinNeededCount * coin.Value);
                                        break;
                                    }
                                }
                            }
                        }

                        if (machine.ClientBalance != 0)
                            continue;
                        else
                        {
                            ctx.SaveChanges();
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        private void ChangeCoinWriteOff(int coinId, int count, MachineContext ctx)
        {
            var coin = ctx.Coins.FirstOrDefault(c => c.Id == coinId);
            coin.TotalCount -= count;
            ctx.SaveChanges();
        }
    }
}