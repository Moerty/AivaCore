using Aiva.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Core.Database.Functions {
    public class SpamProtection {
        public BlacklistHandler Blacklist;

        public SpamProtection() {
            Blacklist = new BlacklistHandler();
        }

        public class BlacklistHandler {
            public async Task<List<BlacklistedWords>> GetBlacklistedWordsAsync() {
                using (var context = new Context()) {
                    return await context.BlacklistedWords
                        .ToListAsync()
                        .ConfigureAwait(false);
                }
            }

            public async Task AddWordAsync(string wordToAdd) {
                if (!string.IsNullOrEmpty(wordToAdd)) {
                    using (var context = new Context()) {
                        var word = new BlacklistedWords { Word = wordToAdd };
                        await context.BlacklistedWords.AddAsync(word)
                            .ConfigureAwait(false);

                        await context.SaveChangesAsync()
                            .ConfigureAwait(false);
                    }
                }
            }

            public async Task RemoveWordAsync(string word) {
                if (!string.IsNullOrEmpty(word)) {
                    using (var context = new Context()) {
                        var entry = await context.BlacklistedWords
                            .FirstOrDefaultAsync(w => w.Word == word)
                            .ConfigureAwait(false);

                        if (entry != null) {
                            context.BlacklistedWords.Remove(entry);

                            await context.SaveChangesAsync()
                                .ConfigureAwait(false);
                        }
                    }
                }
            }

            public async Task<bool> IsWordInListAsync(string word, bool caseSensetive) {
                if (!string.IsNullOrEmpty(word)) {
                    using (var context = new Context()) {
                        return await context.BlacklistedWords
                            .AnyAsync(w => string.Equals(word, w.Word, caseSensetive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase));
                    }
                } else {
                    return false;
                }
            }

            public async Task<bool> IsBlacklistedWordInMessageAsync(string message) {
                if (!string.IsNullOrEmpty(message)) {
                    using (var context = new Context()) {
                        return await context.BlacklistedWords
                            .AnyAsync(word => message.Contains(word.Word));
                    }
                } else {
                    return false;
                }
            }
        }
    }
}
