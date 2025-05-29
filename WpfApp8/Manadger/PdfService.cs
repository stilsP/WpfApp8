using Microsoft.Win32;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp8.Entities;


namespace WpfApp8.Manadger
{
    public static class PdfService
    {
        public static void GenerateReceipt(Order order, Users manager)
        {
            // Создаем диалог сохранения файла
            var saveDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = $"Чек_{order.Code}_{DateTime.Now:yyyyMMdd}.pdf"
            };

            // Если пользователь выбрал место сохранения
            if (saveDialog.ShowDialog() == true)
            {
                // Создаем документ
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Content().Column(column =>
                        {
                            column.Item().Text("Чек продажи").Bold().FontSize(16);
                            column.Item().Text($"Дата: {order.Date:dd.MM.yyyy HH:mm}");
                            column.Item().Text($"Товары: {order.ProductArticle} Кол-во {order.Quantity}");
                            column.Item().Text($"Менеджер: {manager.Name} {manager.Surname}");
                            column.Item().Text($"Клиент: {order.Clients?.Name ?? "Не указан"}");

                        });
                    });
                });

                // Генерируем PDF
                document.GeneratePdf(saveDialog.FileName);

                // Открываем созданный файл
                System.Diagnostics.Process.Start(saveDialog.FileName);
            }
        }
    }
}