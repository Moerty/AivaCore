using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Core.Database.Functions {
    internal class Commands {

        /// <summary>
        /// Add a command to the database
        /// </summary>
        /// <param name="creater">Name of creater</param>
        /// <param name="commandName">Name of the command</param>
        /// <param name="text">Text that will be in chat</param>
        /// <returns></returns>
        internal async Task AddCommand(string creater, string commandName, string text) {
            if (!string.IsNullOrEmpty(creater) && !string.IsNullOrEmpty(commandName) && !string.IsNullOrEmpty(text)) {
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

        /// <summary>
        /// Increate command count
        /// </summary>
        /// <param name="commandName">Name of the command</param>
        /// <returns></returns>
        internal async Task IncreateCommandCount(string commandName) {
            if (!string.IsNullOrEmpty(commandName)) {
                using (var context = new Context()) {
                    var command = await context.Commands
                        .SingleOrDefaultAsync(c => c.Name == commandName)
                        .ConfigureAwait(false);

                    if (command != null) {
                        command.Stack++;

                        await context.SaveChangesAsync()
                            .ConfigureAwait(false);
                    }
                }
            }
        }

        internal async Task EditCommand(string commandName, string text) {
            if (!string.IsNullOrEmpty(commandName) && !string.IsNullOrEmpty(text)) {
                using (var context = new Context()) {
                    var command = await context.Commands
                        .SingleOrDefaultAsync(c => c.Name == commandName)
                        .ConfigureAwait(false);

                    if (command != null) {
                        command.Text = text;

                        await context.SaveChangesAsync()
                            .ConfigureAwait(false);
                    }
                }
            }
        }

        internal async Task RemoveCommand(string commandName) {
            if (!string.IsNullOrEmpty(commandName)) {
                using (var context = new Context()) {
                    var command = await context.Commands
                        .SingleOrDefaultAsync(c => c.Name == commandName)
                        .ConfigureAwait(false);

                    if (command != null) {
                        context.Commands.Remove(command);

                        await context.SaveChangesAsync()
                            .ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
