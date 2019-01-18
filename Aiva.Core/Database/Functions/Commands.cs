using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aiva.Core.Database.Functions {
    internal class Commands {
        internal async void AddCommand(string creater, string commandName, string text) {
            using (var context = new Context()) {
                var dbCommand = await context.Commands.SingleOrDefaultAsync(c => c.Name == commandName)
                    .ConfigureAwait(false);

                if (dbCommand == null) {
                    await context.Commands.AddAsync(
                        new Models.Database.Commands {
                            Stack = 0,
                            CreatedAt = DateTime.Now,
                            CreatedFrom = creater,
                            Name = commandName,
                            Text = text
                        })
                        .ConfigureAwait(false);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
