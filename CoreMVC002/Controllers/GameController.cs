using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMVC002.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreMVC002.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            // 建立新的遊戲引擎
            var model = new XAXBEngine();
            return View(model);
        }

        [HttpPost]
        public ActionResult Guess(XAXBEngine model, string restart)
        {
            // 如果按下重新開始按鈕
            if (!string.IsNullOrEmpty(restart) && restart == "true")
            {
                model.Reset();
                return View("Index", model);
            }

            // 呼叫引擎執行一次猜測，會自動記錄歷史與結果
            model.MakeGuess(model.Guess);

            // 若遊戲結束 (猜中)，在 View 顯示提示
            if (model.IsGameOver(model.Guess))
            {
                ViewBag.GameOver = true;
            }

            return View("Index", model);
        }
    }
}

