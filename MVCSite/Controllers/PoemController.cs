using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using ProjetosViniciusVieiraMota.Models;
using ProjetosViniciusVieiraMota.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProjetosViniciusVieiraMota.Controllers
{
    public class PoemController : Controller
    {
        private readonly SmtpService _smtpService;
        private readonly IServiceProvider _context;

        public PoemController (SmtpService smtpService, IServiceProvider context)
        {
            _smtpService = smtpService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PerformCRD(string query, string actionName)
        {
            if (string.IsNullOrEmpty(query))
            {
                TempData["Message"] = "Insira um e-mail válido";
                return RedirectToAction("Index");
            }
            if (!query.Contains("@"))
            {
                TempData["Message"] = "Insira um e-mail válido";
                return RedirectToAction("Index");
            }

            switch (actionName)
            {
                case "Create":
                    Console.WriteLine("Enviando");
                    _smtpService.Main(query);
                    TempData["Message"] = "Enviado! Por favor, cheque seu e-mail!";
                    EmailModel newUser = new EmailModel();
                    newUser.email = query;
                    await Create(newUser);
                    return RedirectToAction("Index");

                case "Read":
                    Console.WriteLine("Consultando...");
                    await Search(query);
                    return RedirectToAction("Index");

                case "Delete":
                    Console.WriteLine("Deletando...");
                    await Delete(query);
                    return RedirectToAction("Index");

                default:
                    TempData["Message"] = "Ação não reconhecida";
                    break;
            }
           
            return RedirectToAction("Index");
        }  


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmailModel emailModel)
        {

            if (ModelState.IsValid)
            {
                using (var dbContext = _context.CreateScope().ServiceProvider.GetRequiredService<DBContextService>()) // Use dependency injection to get a new DBContextService instance
                {
                    try
                    {
                        dbContext.emails.Add(emailModel); // Adiciona o modelo ao contexto do banco de dados
                        await dbContext.SaveChangesAsync(); // Salva as alterações no banco de dados
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        throw;
                    }
                }
            }
            return View(emailModel);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            using (var dbContext = _context.CreateScope().ServiceProvider.GetRequiredService<DBContextService>())
                try
                {
                    EmailModel email = await dbContext.FindAsync<EmailModel>(query);
                    if (email != null)
                    {
                        TempData["Message"] = "Encontrado! O e-mail é: " + email.email;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Delete (string query)
        {
            using (var dbContext = _context.CreateScope().ServiceProvider.GetRequiredService<DBContextService>())
                try
                {
                    EmailModel email = await dbContext.FindAsync<EmailModel>(query);
                    if (email != null)
                    {
                        dbContext.Remove(email);
                        await dbContext.SaveChangesAsync(); // Salve as alterações no banco de dados
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            return View();
        }



    }
}
