using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotProject.Modules
{
    public class Test : ModuleBase<SocketCommandContext>
    {
        DateTime oDate;
        string teams;
        public class AnalysisJson
        {
            public string Sector { get; set; }
            public string Analysis1 { get; set; }
            public string Analysis2 { get; set; }
            public string Analysis3 { get; set; }
        }
        public class DateTimes
        {
            public DateTime Time { get; set; }
        }
        public class TeamsJson
        {
            public string Name { get; set; }
            public string Gumball1 { get; set; }
            public string Gumball2 { get; set; }
            public string Gumball3 { get; set; }
            public string Potion { get; set; }
            public string Artifact { get; set; }
            public string Description { get; set; }
        }

        [Command("test2")]//Just for tests
        public async Task commandTest2()
        { //fdkjngkdjsg
            EmbedBuilder builder = new EmbedBuilder();
            builder.WithAuthor(Context.User);
            builder.WithTitle("Gumball Team Test");
            builder.AddField("Adventurer", "<:Adventurer:742741303254581258>", true);
            builder.AddField("Little May", "<:little_may:742742508672647268>", true);
            builder.AddField("Swordsman", "<:swordsman:742742127414476890>", true);
            builder.AddField("Harp of Adventurer", "<:harp_of_adventurer:742761691552677920>", true);
            builder.AddField("Saints stare", "<:saints_stare:742761419199610919>", true);
            builder.AddField("Description", "this is a description");
            builder.WithImageUrl("https://cdn.discordapp.com/attachments/475255133983211520/710273172423049307/Screenshot_2020-05-14-06-27-19-57.png");

            builder.WithColor(Color.Red);
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
        [Command("help")] //help command
        public async Task commandHelp()
        {
            await Context.Channel.SendMessageAsync("Type one of these commands to see what they do! \n !igt \n !reset \n !analysis \n !boink \n !team \n !T3");
        }
        [Command("igt")] //check in game time
        public async Task InGameTime()
        {
            var date = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, date);
            await Context.Channel.SendMessageAsync(localTime.ToString());
        }
        [Command("reset")] //check next reset
        public async Task Reset()
        {
            var date = DateTime.Now;
            var DateTime8 = date.Date.AddHours((date.Hour > 17) ? 24 + 17 : 17);
            TimeSpan diff = DateTime8 - date;
            if (diff.Hours >= 0 && diff.Minutes >= 0 && diff.Seconds >= 0)
                await Context.Channel.SendMessageAsync("Game will reset in " + diff.Hours.ToString() + "H " + diff.Minutes.ToString() + "M " + diff.Seconds.ToString() + "S ");
            else
            {
                date.AddDays(-1);
                diff = DateTime8 - date;
                await Context.Channel.SendMessageAsync("Game will reset in " + diff.Hours.ToString() + "H " + diff.Minutes.ToString() + "M " + diff.Seconds.ToString() + "S ");
            }
        }


        [Command("analysis")] //check analysis
        public async Task Analysis(string sector)
        {
            string json = System.IO.File.ReadAllText(@"C:\Users\Dot4nuki\source\repos\DiscordBotProject\DiscordBotProject\JSON\Analysis.json");
            var analysis = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AnalysisJson>>(json);
            var selectSector = from s in analysis where s.Sector == sector select s;
            if (selectSector.First().Analysis2 == "")
                await Context.Channel.SendMessageAsync("For " + sector + " you need **" + selectSector.First().Analysis1 + "**");
            else if (selectSector.First().Analysis3 == "")
                await Context.Channel.SendMessageAsync("For " + sector + " you need **" + selectSector.First().Analysis1 + "** and **" + selectSector.First().Analysis2 + "**");
            else
                await Context.Channel.SendMessageAsync("For " + sector + " you need **" + selectSector.First().Analysis1 + "** and **" + selectSector.First().Analysis2 + "** and **" + selectSector.First().Analysis3 + "**");
        }
        [Command("analysis")] //fail (no parameters)
        public async Task AnalysisFail()
        {
            await Context.Channel.SendMessageAsync("Check the analysis needed to get the sector's secrets: !analysis M06");
        }


        [Command("boink")] //set new boink
        public async Task boinkTime(string day, string hour)
        {
            string time = day + " " + hour;
            try
            {
                oDate = DateTime.ParseExact(time, "yyyy/M/d H:mm", null);
            }
            catch
            {
                await Context.Channel.SendMessageAsync("Invalid date. Example use: !boink 2020/12/3 5:30");
                return;
            }

            List<DateTimes> _data = new List<DateTimes>();
            _data.Add(new DateTimes()
            {
                Time = oDate,
            });
            string json = JsonConvert.SerializeObject(_data.ToArray());
            System.IO.File.WriteAllText(@"C:\Users\Dot4nuki\source\repos\DiscordBotProject\DiscordBotProject\JSON\BoinkTime.json", json);
            await Context.Channel.SendMessageAsync("New Boink time set to: " + oDate.Day + "/" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(oDate.Month) + " " + oDate.Hour + ":" + oDate.Minute);
        }
        [Command("boink")] //check boink
        public async Task boinkTimeCheck(string check)
        {
            if (check == "check")
            {
                string json = System.IO.File.ReadAllText(@"C:\Users\Dot4nuki\source\repos\DiscordBotProject\DiscordBotProject\JSON\BoinkTime.json");
                var boink = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DateTimes>>(json);
                await Context.Channel.SendMessageAsync(boink.First().Time.Day.ToString() + "/" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(boink.First().Time.Month) + " " + boink.First().Time.Hour + ":" + boink.First().Time.Minute);
            }
            else
            {
                await Context.Channel.SendMessageAsync("Error! To check boink time use: !boink check");
            }
        }
        [Command("boink")] //fail (no parameters)
        public async Task boinkFail()
        {
            await Context.Channel.SendMessageAsync("Manage boink parties with this command:\n -Set New boink party: !boink 2020/9/21 5:45 (respect spaces!)\n -Check boink party time: !boink check \n Also, times should be added in **UTC** to avoid confusion");
        }


        [Command("T3")] //set new T3
        public async Task tier3(string day, string hour)
        {
            string time = day + " " + hour;
            try
            {
                oDate = DateTime.ParseExact(time, "yyyy/M/d H:mm", null);
            }
            catch
            {
                await Context.Channel.SendMessageAsync("Invalid date. Example use: !T3 2020/12/3 5:30");
                return;
            }

            List<DateTimes> _data = new List<DateTimes>();
            _data.Add(new DateTimes()
            {
                Time = oDate,
            });
            string json = JsonConvert.SerializeObject(_data.ToArray());
            System.IO.File.WriteAllText(@"C:\Users\Dot4nuki\source\repos\DiscordBotProject\DiscordBotProject\JSON\Tier3.json", json);
            await Context.Channel.SendMessageAsync("New Tier3 time set to: " + oDate.Day + "/" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(oDate.Month) + " " + oDate.Hour + ":" + oDate.Minute);
        }
        [Command("T3")] //check T3
        public async Task Tier3TimeCheck(string check)
        {
            if (check == "check")
            {
                string json = System.IO.File.ReadAllText(@"C:\Users\Dot4nuki\source\repos\DiscordBotProject\DiscordBotProject\JSON\Tier3.json");
                var tier = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DateTimes>>(json);
                await Context.Channel.SendMessageAsync(tier.First().Time.Day.ToString() + "/" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(tier.First().Time.Month) + " " + tier.First().Time.Hour + ":" + tier.First().Time.Minute);
            }
            else
            {
                await Context.Channel.SendMessageAsync("Error! To check Tier3 use: !T3 check");
            }
        }
        [Command("T3")] //fail (no parameters)
        public async Task Tier3Fail()
        {
            await Context.Channel.SendMessageAsync("Manage Tier3 parties with this command:\n -Set New tier3 party: !T3 2020/9/21 5:45 (respect spaces!)\n -Check tier3 party time: !T3 check \n Also, times should be added in **UTC** to avoid confusion");
        }


        [Command("team")] //add team
        public async Task createTeam(string name, string gumball1, string gumball2, string gumball3, string potion, string artifact, string description)
        {
            // Read existing json data
            var jsonData = System.IO.File.ReadAllText(@"C:\Users\Dot4nuki\source\repos\DiscordBotProject\DiscordBotProject\JSON\Teams.json");
            // De-serialize to object or create new list
            var teamList = JsonConvert.DeserializeObject<List<TeamsJson>>(jsonData)
                                  ?? new List<TeamsJson>();

            // Add team
            teamList.Add(new TeamsJson()
            {
                Name = name,
                Gumball1 = gumball1,
                Gumball2 = gumball2,
                Gumball3 = gumball3,
                Potion = potion,
                Artifact = artifact,
                Description = description
            });

            // Update json data string
            jsonData = JsonConvert.SerializeObject(teamList);
            System.IO.File.WriteAllText(@"C:\Users\Dot4nuki\source\repos\DiscordBotProject\DiscordBotProject\JSON\Teams.json", jsonData);
            await Context.Channel.SendMessageAsync("The team has been added successfully!");
        }
        [Command("team")] //view team names & check team
        public async Task viewAndCheckTeams(string check)
        {
            if (check == "check")
            {
                string json = System.IO.File.ReadAllText(@"C:\Users\Dot4nuki\source\repos\DiscordBotProject\DiscordBotProject\JSON\Teams.json");
                var jsonTeams = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TeamsJson>>(json);
                foreach (var item in jsonTeams)
                {
                    teams += item.Name + "; ";
                }
                await Context.Channel.SendMessageAsync("These are the team names: " + teams);
            }
            else
            {
                string json = System.IO.File.ReadAllText(@"C:\Users\Dot4nuki\source\repos\DiscordBotProject\DiscordBotProject\JSON\Teams.json"); //search in json file
                var jsonTeams = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TeamsJson>>(json);
                var selectTeam = from t in jsonTeams where t.Name == check select t;

                var test = Context.Guild.Emotes; //search emojis
                var gumball1 = from t in test where t.Name == selectTeam.First().Gumball1 select t;
                var gumball2 = from t in test where t.Name == selectTeam.First().Gumball2 select t;
                var gumball3 = from t in test where t.Name == selectTeam.First().Gumball3 select t;
                var potion = from t in test where t.Name == selectTeam.First().Potion select t;
                var artifact = from t in test where t.Name == selectTeam.First().Artifact select t;

                await Context.Channel.SendMessageAsync("**" + selectTeam.First().Name + "**");
                await Context.Channel.SendMessageAsync(gumball1.First() + "\t\t" + gumball2.First() + "\t\t" + gumball3.First());
                await Context.Channel.SendMessageAsync(potion.First() + "\t\t" + artifact.First());
                await Context.Channel.SendMessageAsync("**\n Description: **" + selectTeam.First().Description);
            }
        }
        [Command("team")] //fail (no parameters)
        public async Task TeamFail()
        {
            await Context.Channel.SendMessageAsync("Manage teams with this command! \n -Create a team using: !team (TeamName) (MainGumball) (2ndGumball) (3rdGumball) (Potion) (Artifact) (Description) \n -To view other teams created use: !team (TeamName) \n -To get all team names use: !team check");
        }
    }
}
