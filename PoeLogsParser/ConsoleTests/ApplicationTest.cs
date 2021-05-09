using System.Collections.Generic;
using System.Text;
using PoeLogsParser.Services;

namespace ConsoleTests
{
    public class ApplicationTest
    {
        public void Run()
        {
            var logs = new List<string>()
            {
                "2020/07/26 18:51:07 11204828 b60 [INFO Client 12372] #HarcorDebil: ALL END GAME BOSSES SERVICE FOR TIPS AND CHALLANGES(PM FOR CHALLANGES INFO)(EXCPET ATZIRI,HOGM)",
                "2020/07/26 15:54:03 581390 b60 [INFO Client 12372] : Joopathedead has joined the area.",
                "2020/07/25 16:55:50 176986234 b60 [INFO Client 20380] : You have entered Karui Shores.",
                "2020/07/25 16:54:54 ***** LOG FILE OPENING *****",
                "2020/07/25 16:54:54 176929968 8a [INFO Client 20380] Enumerated adapter: NVIDIA GeForce GTX 960",
                "2020/07/25 16:54:54 176930125 929 [INFO Client 20380] [DEVICE] Type: Vulkan",
                "2020/07/25 16:54:54 176930156 1be [INFO Client 20380] [VULKAN] Supported instance extension: VK_KHR_external_memory_capabilities",
                "2021/01/17 17:41:02 20911781 bb2 [INFO Client 38500] @To animepsychoo: Hi, I'd like to buy your 1 Exalted Orb for my 75 Chaos Orb in Ritual.",
                "2021/01/17 13:45:41 6791437 bb2 [INFO Client 11380] @To PopiPlantedTheBomb: Hi, I would like to buy your Eagle Glisten Large Cluster Jewel listed for 9 chaos in Ritual (stash tab \"~price 1 chaos\"; position: left 6, top 12)",
                "2021/01/16 20:54:13 23216640 bb2 [INFO Client 22676] @From DickerPrügel: Hi, I'd like to buy your 1 Foreboding Delirium Orb for my 2 Chaos Orb in Ritual.",
                "2021/01/16 14:29:01 103937 bb2 [INFO Client 7352] @From Blasterzor: Hi, I would like to buy your The Celestial Justicar listed for 1 chaos in Ritual (stash tab \"~price 1 chaos\"; position: left 6, top 12)"
            };
            var lps = new LogParserService();

            foreach (var log in logs)
            {
                var result = lps.Parse(log);
                var g = 0;
            }
        }
    }
}