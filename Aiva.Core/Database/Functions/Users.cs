using Aiva.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api.V5.Models.Users;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace Aiva.Core.Database.Functions {
    internal class Users {
        public readonly Add AddUser;
        public readonly Remove RemoveUser;
        public readonly Get GetUser;

        public Users() {
            AddUser = new Add();
            RemoveUser = new Remove();
            GetUser = new Get();
        }

        internal class Add {
            internal async Task AddUserToDatabaseAsync(User twitchUser) {
                if (twitchUser != null) {
                    using (var context = new Context()) {
                        var dbUser = await context.Users
                            .Include(active => active.ActiveUsers)
                            .SingleOrDefaultAsync(u => u.UsersId == twitchUser.Id)
                            .ConfigureAwait(false);

                        // update user if exist in database
                        if (dbUser != null) {
                            dbUser.Bio = twitchUser.Bio;
                            dbUser.CreatedAt = twitchUser.CreatedAt;
                            dbUser.DisplayName = twitchUser.DisplayName;
                            dbUser.Logo = twitchUser.Logo;
                            dbUser.Name = twitchUser.Name;
                            dbUser.Type = twitchUser.Type;
                            dbUser.UpdatedAt = twitchUser.UpdatedAt;

                            // Active users
                            if (dbUser.ActiveUsers == null) {
                                dbUser.ActiveUsers = new ActiveUsers {
                                    UsersId = dbUser.UsersId,
                                    JoinedTime = DateTime.Now,
                                    Users = dbUser,
                                };
                            }
                        } else {
                            // create new user

                            var newUser = new Models.Database.Users {
                                UsersId = twitchUser.Id,
                                Bio = twitchUser.Bio,
                                CreatedAt = twitchUser.CreatedAt,
                                DisplayName = twitchUser.DisplayName,
                                Logo = twitchUser.Logo,
                                Name = twitchUser.Name,
                                Type = twitchUser.Type,
                                UpdatedAt = twitchUser.UpdatedAt,
                                Currency = new Models.Database.Currency {
                                    UsersId = twitchUser.Id,
                                    Value = 0,
                                },
                                ActiveUsers = new ActiveUsers {
                                    UsersId = twitchUser.Id,
                                    JoinedTime = DateTime.Now,
                                },
                                TimeWatched = new TimeWatched {
                                    UsersId = twitchUser.Id,
                                    Time = 0,
                                }
                            };

                            await context.Users.AddAsync(newUser)
                                .ConfigureAwait(false);
                        }

                        await context.SaveChangesAsync()
                            .ConfigureAwait(false);
                    }
                }
            }

            internal async void UserJoined(object sender, OnUserJoinedArgs e) {
                var twitchUser = await Twitch.AivaClient.TwitchApi.V5.Users.GetUserByNameAsync(e.Username)
                        .ConfigureAwait(false);

                if (twitchUser?.Total > 0) {
                    await AddUserToDatabaseAsync(twitchUser.Matches[0]);
                }
            }

            internal async void AddExistingUsers(object sender, OnExistingUsersDetectedArgs e) {
                foreach(var user in e.Users) {
                    var twitchUser = await Twitch.AivaClient.TwitchApi.V5.Users.GetUserByNameAsync(user)
                        .ConfigureAwait(false);

                    if(twitchUser?.Total > 0) {
                        await AddUserToDatabaseAsync(twitchUser.Matches[0]);
                    }
                }
            }

            internal async Task AddUserToDatabaseAsync(ChatMessage message) {
                if(message != null) {
                    using (var context = new Context()) {
                        var dbUser = await context.Users
                            .Include(active => active.ActiveUsers)
                            .SingleOrDefaultAsync(u => u.UsersId == message.UserId)
                            .ConfigureAwait(false);

                        // update user if exist in database
                        if (dbUser == null) {
                            var newUser = new Models.Database.Users {
                                UsersId = message.UserId,
                                DisplayName = message.DisplayName,
                                Currency = new Models.Database.Currency {
                                    UsersId = message.UserId,
                                    Value = 0,
                                },
                                ActiveUsers = new ActiveUsers {
                                    UsersId = message.UserId,
                                    JoinedTime = DateTime.Now,
                                },
                                TimeWatched = new TimeWatched {
                                    UsersId = message.UserId,
                                    Time = 0,
                                }
                            };

                            await context.Users.AddAsync(newUser)
                                .ConfigureAwait(false);

                            await context.SaveChangesAsync()
                                .ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        internal class Remove {
            internal async void UserLeft(object sender, OnUserLeftArgs e) {
                var twitchUser = await Twitch.AivaClient.TwitchApi.V5.Users.GetUserByNameAsync(e.Username)
                        .ConfigureAwait(false);

                if(twitchUser?.Total > 0) {
                    using(var context = new Context()) {
                        var dbUser = await context.Users
                            .Include(active => active.ActiveUsers)
                            .Include(watched => watched.TimeWatched)
                            .SingleOrDefaultAsync(user => user.UsersId == twitchUser.Matches[0].Id);

                        if(dbUser != null) {
                            dbUser.TimeWatched.Time += DateTime.Now.Subtract(dbUser.ActiveUsers.JoinedTime).Ticks;

                            context.ActiveUsers.Remove(dbUser.ActiveUsers);

                            await context.SaveChangesAsync();
                        }
                    }
                    //var dbUser = 
                }
            }
        }

        internal class Get {
            internal async Task<bool> IsUserInDatabase(string userId) {
                using(var context = new Context()) {
                    return await context.Users.AnyAsync(u => u.UsersId == userId)
                        .ConfigureAwait(false);
                }
            }
        }
    }
}
