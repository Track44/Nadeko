using Discord;
using Discord.Commands;
using NadekoBot.Attributes;
using NadekoBot.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NadekoBot.Modules.Utility
{
    public partial class Utility
    {
        [Group]
        public class InfoCommands : ModuleBase
        {
            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            public async Task ServerInfo(string guildName = null)
            {
            var channel = (ITextChannel)Context.Channel;
            guildName = guildName?.ToUpperInvariant();
            IGuild guild;
            if (string.IsNullOrWhiteSpace(guildName))
                guild = channel.Guild;
            else
                guild = NadekoBot.Client.GetGuilds().Where(g => g.Name.ToUpperInvariant() == guildName.ToUpperInvariant()).FirstOrDefault();
            if (guild == null)
                return;
            var ownername = await guild.GetUserAsync(guild.OwnerId);
            var textchn = (await guild.GetTextChannelsAsync()).Count();
            var voicechn = (await guild.GetVoiceChannelsAsync()).Count();
            
            var createdAt = new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(guild.Id >> 22);
            DateTime timeNow = DateTime.UtcNow;
            int days = (int)Math.Abs(Math.Round((createdAt - timeNow).TotalDays));
            var sb = new StringBuilder();
            var users = await guild.GetUsersAsync().ConfigureAwait(false);
            var embed = new EmbedBuilder()
                .WithAuthor(eab => eab.WithName("Server Info"))
                .WithTitle(guild.Name)
                .AddField(fb => fb.WithName("**ID**").WithValue(guild.Id.ToString()).WithIsInline(true))
                .AddField(fb => fb.WithName("**Owner**").WithValue(ownername.ToString()).WithIsInline(true))
                .AddField(fb => fb.WithName("**Members**").WithValue(users.Count.ToString()).WithIsInline(true))
                .AddField(fb => fb.WithName("**Text Channels**").WithValue(textchn.ToString()).WithIsInline(true))
                .AddField(fb => fb.WithName("**Voice Channels**").WithValue(voicechn.ToString()).WithIsInline(true))
                .AddField(fb => fb.WithName("**Created At**").WithValue($"{createdAt.ToString("dd.MM.yyyy HH:mm")}").WithIsInline(true))
                .AddField(fb => fb.WithName("**Days Since Creation**").WithValue(days.ToString()).WithIsInline(true))
                .AddField(fb => fb.WithName("**Region**").WithValue(guild.VoiceRegionId.ToString()).WithIsInline(true))
                .AddField(fb => fb.WithName("**Roles**").WithValue(guild.Roles.Count().ToString()).WithIsInline(true))
                .AddField(fb => fb.WithName("**Icon Link**").WithValue($"[Link]({guild.IconUrl.ToString()})").WithIsInline(true))
                .WithThumbnailUrl(guild.IconUrl)
                .WithOkColor();
            if (guild.Emojis.Count() > 0)
            {
                embed.AddField(fb => fb.WithName("**Custom Emojis**").WithValue(Format.Italics(string.Join(", ", guild.Emojis))).WithIsInline(false));
            }
            await Context.Channel.EmbedAsync(embed).ConfigureAwait(false);
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            public async Task ChannelInfo(ITextChannel channel = null)
            {
                var ch = channel ?? (ITextChannel)Context.Channel;
                if (ch == null)
                    return;
                var createdAt = new DateTime(2015, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(ch.Id >> 22);
                DateTime timeNow = DateTime.UtcNow;
                int days = (int)Math.Abs(Math.Round((createdAt - timeNow).TotalDays));
                var usercount = (await ch.GetUsersAsync().Flatten()).Count();
                await Context.Channel.EmbedAsync(new EmbedBuilder()
                    .WithTitle(ch.Name)
                    .WithDescription(ch.Topic?.SanitizeMentions())
                    .AddField(fb => fb.WithName("**ID**").WithValue(ch.Id.ToString()).WithIsInline(true))
                    .AddField(fb => fb.WithName("**Created At**").WithValue($"{createdAt.ToString("dd.MM.yyyy HH:mm")}").WithIsInline(true))
                    .AddField(fb => fb.WithName("**Days Since Creation**").WithValue(days.ToString()).WithIsInline(true))
                    .AddField(fb => fb.WithName("**Users**").WithValue(usercount.ToString()).WithIsInline(true))
                    .WithOkColor()
                    ).ConfigureAwait(false);
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            public async Task UserInfo(IGuildUser usr = null)
            {
                try {
                    var channel = (ITextChannel)Context.Channel;
                    var user = usr ?? Context.User as IGuildUser;

                    if (user == null)
                        return;

                    var createdA = (DateTimeOffset)user.CreatedAt;
                    var joinedA = (DateTimeOffset)user.JoinedAt;
                    DateTime createdAt = createdA.UtcDateTime;
                    DateTime joinedAt = joinedA.UtcDateTime;
                    DateTime timeNow = DateTime.UtcNow;
                    int daysCreation = (int)Math.Abs(Math.Round((createdAt - timeNow).TotalDays));
                    int daysJoin = (int)Math.Abs(Math.Round((joinedAt - timeNow).TotalDays));
                    string avi = user.AvatarUrl;
                    if (user.AvatarId.Substring(0,2) == "a_")
                        avi = avi.Substring(0, avi.Length-3) + "gif";
                    if (user.JoinedAt?.ToString("dd.MM.yyyy HH:mm") == "01.01.0001 00:00") {
                        await Context.Channel.SendMessageAsync($">userinfo {user.Mention}");
                    }
                    var embed = new EmbedBuilder()
                        .AddField(fb => fb.WithName("**Name**").WithValue($"**{user.Username}**#{user.Discriminator}").WithIsInline(true));
                    if (!string.IsNullOrWhiteSpace(user.Nickname))
                    {
                        embed.AddField(fb => fb.WithName("**Nickname**").WithValue(user.Nickname).WithIsInline(true));
                    }
                    embed.AddField(fb => fb.WithName("**ID**").WithValue(user.Id.ToString()).WithIsInline(true));
                    if (user.JoinedAt?.ToString("dd.MM.yyyy HH:mm") != "01.01.0001 00:00") {
                        embed.AddField(fb => fb.WithName("**Joined Server**").WithValue($"{user.JoinedAt?.ToString("dd.MM.yyyy HH:mm")}").WithIsInline(true))
                        .AddField(fb => fb.WithName("**Days Since Joined**").WithValue(daysJoin.ToString()).WithIsInline(true));
                    }
                    embed.AddField(fb => fb.WithName("**Created At**").WithValue($"{user.CreatedAt.ToString("dd.MM.yyyy HH:mm")}").WithIsInline(true))
                        .AddField(fb => fb.WithName("**Days Since Created**").WithValue(daysCreation.ToString()).WithIsInline(true))
                        .AddField(fb => fb.WithName("**Current Game**").WithValue($"{(user.Game?.Name == null ? "-" : user.Game.Value.Name)}").WithIsInline(true))
                        .AddField(fb => fb.WithName("**Roles**").WithValue($"**({user.RoleIds.Count})** - {string.Join(", ", user.GetRoles().Select(r => r.Name)).SanitizeMentions()}").WithIsInline(true))
                        .WithThumbnailUrl(avi)
                        .WithOkColor();
                    await Context.Channel.EmbedAsync(embed).ConfigureAwait(false);
                }
                catch { await Context.Channel.SendMessageAsync("Something went wrong, it's not your fault, blame the discord.NET API ¯\\_(ツ)_/¯").ConfigureAwait(false); }
            }
        }

        [NadekoCommand, Usage, Description, Aliases]
        [RequireContext(ContextType.Guild)]
        [OwnerOnly]
        public async Task Activity(int page = 1)
        {
            const int activityPerPage = 15;
            page -= 1;

            if (page < 0)
                return;

            int startCount = page * activityPerPage;

            StringBuilder str = new StringBuilder();
            foreach (var kvp in NadekoBot.CommandHandler.UserMessagesSent.OrderByDescending(kvp => kvp.Value).Skip(page*activityPerPage).Take(activityPerPage))
            {
                str.AppendLine($"`{++startCount}.` **{kvp.Key}** [{kvp.Value/NadekoBot.Stats.GetUptime().TotalSeconds:F2}/s] - {kvp.Value} total");
            }

            await Context.Channel.EmbedAsync(new EmbedBuilder().WithTitle($"Activity Page #{page}")
                .WithOkColor()
                .WithFooter(efb => efb.WithText($"{NadekoBot.CommandHandler.UserMessagesSent.Count} users total."))
                .WithDescription(str.ToString()));
        }
    }
}
