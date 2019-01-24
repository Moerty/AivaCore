using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Core.Database.Functions {
    public class Currency {
        #region Models
        public AddCurrency Add;
        public RemoveCurrency Remove;
        public TransferCurrency Transfer;
        #endregion Models

        #region Constructor
        public Currency() {
            Add = new AddCurrency();
            Remove = new RemoveCurrency();
            Transfer = new TransferCurrency();
        }
        #endregion Constructor

        #region Add
        public class AddCurrency {
            /// <summary>
            /// Add list of Users Currency
            /// </summary>
            /// <param name="UserList">todo: describe UserList parameter on AddCurrencyToUserList</param>
            public async Task Add(List<Tuple<string, string, int>> UserList) {
                if (UserList?.Count > 0) {
                    using (var context = new Context()) {
                        foreach (var user in UserList) {
                            if (!string.IsNullOrEmpty(user.Item1) && !string.IsNullOrEmpty(user.Item2)) {
                                var userDb = await context.Users
                                    .SingleOrDefaultAsync(u => u.UsersId == user.Item2)
                                    .ConfigureAwait(false);

                                if (userDb != null) {
                                    userDb.Currency.Value += user.Item3;
                                }
                            }
                        }

                        await context.SaveChangesAsync()
                            .ConfigureAwait(false);
                    }
                }
            }

            public async Task<bool> Add(string twitchID, int value) {
                using (var context = new Context()) {
                    var user = await context.Users
                        .Include(c => c.Currency)
                        .SingleOrDefaultAsync(u => u.UsersId == twitchID)
                        .ConfigureAwait(false);

                    if (user != null) {
                        user.Currency.Value += value;

                        await context.SaveChangesAsync()
                            .ConfigureAwait(false);

                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Add currency to active viewers
            /// </summary>
            public async Task AddCurrencyActiveViewer() {
                using (var context = new Context()) {
                    var activeUsers = await context.ActiveUsers
                        .ToListAsync()
                        .ConfigureAwait(false);

                    foreach (var user in activeUsers) {
                        user.Users.Currency.Value += Convert.ToInt64(ConfigHandler.Config.Currency.CurrencyToAddFrequently);
                    }

                    await context.SaveChangesAsync()
                        .ConfigureAwait(false);
                }
            }
        }
        #endregion Add

        #region Remove

        public class RemoveCurrency {
            /// <summary>
            /// Remove Currency from User
            /// </summary>
            /// <param name="twitchID"></param>
            /// <param name="value"></param>
            public async Task<bool> Remove(string twitchID, int value) {
                using (var context = new Context()) {
                    var user = await context.Users
                        .Include(c => c.Currency)
                        .SingleOrDefaultAsync(u => u.UsersId == twitchID)
                        .ConfigureAwait(false);

                    if (user != null) {
                        user.Currency.Value -= value;

                        await context.SaveChangesAsync()
                            .ConfigureAwait(false);

                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Remove list of Users Currency
            /// </summary>
            /// <param name="UserList"></param>
            public async Task Remove(List<Tuple<string, string, int>> UserList) {
                using (var context = new Context()) {
                    foreach (var user in UserList) {
                        var userDb = await context.Users
                            .SingleOrDefaultAsync(u => u.UsersId == user.Item2)
                            .ConfigureAwait(false);

                        if (userDb != null) {
                            userDb.Currency.Value -= user.Item3;
                        }
                    }

                    await context.SaveChangesAsync()
                        .ConfigureAwait(false);
                }
            }
        }
        #endregion Remove

        #region Transfer

        public class TransferCurrency {
            /// <summary>
            /// Transfer currency from a user to a user
            /// </summary>
            /// <param name="userid1"></param>
            /// <param name="userid2"></param>
            /// <param name="value"></param>
            internal async Task<bool> Transfer(string userid1, string userid2, int value) {
                using (var context = new Context()) {
                    var user1 = await context.Users
                        .SingleOrDefaultAsync(u => u.UsersId == userid1)
                        .ConfigureAwait(false);

                    var user2 = await context.Users
                        .SingleOrDefaultAsync(u => u.UsersId == userid2)
                        .ConfigureAwait(false);

                    if (user1 != null && user2 != null) {
                        if (user1.Currency.Value >= value) {
                            user1.Currency.Value -= value;
                            user2.Currency.Value += value;

                            await context.SaveChangesAsync()
                                .ConfigureAwait(false);

                            return true;
                        }
                    }
                }

                return false;
            }
        }

        #endregion Transfer

        #region Functions

        /// <summary>
        /// Get Currency from a user
        /// </summary>
        /// <param name="twitchID">todo: describe twitchID parameter on GetCurrencyFromUser</param>
        /// <returns></returns>
        public async Task<long?> GetCurrency(string twitchID) {
            using (var context = new Context()) {
                var user = await context.Users
                    .Include(u => u.Currency)
                    .SingleOrDefaultAsync(u => u.UsersId == twitchID)
                    .ConfigureAwait(false);

                if (user != null) {
                    return user.Currency.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if a user has enough currency
        /// </summary>
        /// <param name="twitchID"></param>
        /// <param name="currencyToCheck"></param>
        /// <returns></returns>
        public async Task<bool> HasUserEnoughCurrency(string twitchID, int currencyToCheck) {
            using (var context = new Context()) {
                var user = await context.Users
                    .Include(c => c.Currency)
                    .SingleOrDefaultAsync(u => twitchID == u.UsersId)
                    .ConfigureAwait(false);

                if (user != null) {
                    if (user?.Currency?.Value >= currencyToCheck) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Add User to Table Currency
        /// </summary>
        /// <param name="twitchID"></param>
        public async Task AddUserToCurrencyTable(string twitchID) {
            using (var context = new Context()) {
                var currencyEntry = await context.Currency
                    .SingleOrDefaultAsync(c => c.UsersId == twitchID)
                    .ConfigureAwait(false);

                if (currencyEntry == null) {
                    context.Currency.Add(
                        new Models.Database.Currency {
                            UsersId = twitchID,
                            Value = 0
                        });

                    await context.SaveChangesAsync()
                        .ConfigureAwait(false);
                }
            }
        }

        #endregion Functions
    }
}
