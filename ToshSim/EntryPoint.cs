using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Threading;

namespace toshko_simulator
{
    class ToshSim
    {
        private static readonly string[] ToshkoPhrases = 
        {
            "ти па си мноо убав",
            "ти па си Ален Делон",
            "ни са праи",
            "ще ти счупя пръстчетата",
            "аре да пийм",
            "май ша са бийм",
            "така няма да стане",
            "аре да се вчешем",
            "аз съм тошко",
            "и ся кво праим",
            "време е"
        };

        private static readonly string[] ToshkoPhrasesEnglishized = 
        {
            "tee pa see mnooo ohbaph",
            "tee pa see Alen Delon",
            "nay say pryeeeee",
            "she teey shchupya prast chaytata",
            "array da peeeeeeem",
            "mai she say beeeeeeeem",
            "tacka nyama da stanyee",
            "array the say ffcheshem",
            "as sam toyshcko",
            "e sya cffo pryeem",
            "vreyymey e"
        };

        private static readonly Random Rnd = new Random();

        private static int speechRate = 0;
        private static int speechVolume = 50;

        private static bool autonomous_toshko = true;
        private static int min_delay = 1500;
        private static int max_delay = 4000;

        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static void Main(params string[] args)
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);
            Console.CursorVisible = false;
            Console.TreatControlCAsInput = true;
            Console.WindowHeight = 60;
            Console.WindowWidth = 60;
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;

            try
            {
                autonomous_toshko = bool.Parse(
                    ConfigurationManager.AppSettings["autonomous_toshko"]);
                min_delay = int.Parse(
                    ConfigurationManager.AppSettings["min_delay"]);
                max_delay = int.Parse(
                    ConfigurationManager.AppSettings["max_delay"]);
            }
            catch
            { }

            using (SpeechSynthesizer synth = new SpeechSynthesizer() { Volume = speechVolume, Rate = speechRate })
            {
                synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Teen);

                while (true)
                {
                    Console.Clear();
                    int randomIndex = Rnd.Next(0, ToshkoPhrases.Length);
                    string consoleOutput = ToshkoPhrases[randomIndex];
                    Console.WriteLine(consoleOutput);
                    Console.WriteLine(ToshkoAscii);
                    Console.Title = consoleOutput;
                    synth.Speak(ToshkoPhrasesEnglishized[randomIndex]);

                    if (autonomous_toshko)
                        Thread.Sleep(Rnd.Next(min_delay, max_delay));
                    else
                    {
                        Console.WriteLine("Press enter to continue");
                        Console.ReadLine();
                    }
                }
            }
        }

        private static string ToshkoAscii =
@"                       :;;;;`                             
                    ,@@@@@@@@@'                            
               .;+@@@@@@@@@@@@@@#;                         
              .@@@@@@@@@@@@@@@@@@@@:                       
              @@@@@@@@@@@@#@@@@@@@@@@;                     
             @@@@@@@@@#@##++###@@@@@@@@,                   
            '@@@@@@@@#@+#++++++###@@@@@@                   
           :@@@@@@##+####+++''++###@@@@@#                  
           @@@@@#+'++++++++++'''+###@@@@@+                 
          `@@@@#';:::;';;,,,;;;;'''+##@@@@:                
          :@@@+':,,,,,,,,,,,,,,,,,,,:'#@@@@                
          #@@@+:,,...,,,,,,,,,,,,,,,,,,:;@@`               
          @@@#;,,......,,,,,,,,,,,,,,,,,,#@+               
         ;@@@';...........,,,,,,,,,,,,,,,:@@               
         @@@';,............,,,,,,,,,,,,.,,@@:              
        ;@@@+;,...........,,,,,,,,,,,,.,,,@@;              
        ;@@@+;,...........,,,,,,,,,,,,,,,,@@,              
        .@@@#;,...........,,,,,,,,,,,,,,,,@@               
        `@@@#;,..........,,,,,,,,,,,,,,,,,@#               
        ,@@@+;..........,,,,,,,,,,,,,,,,,:@+               
        +@@#+,..........,,,,,,,,,,.,,,,,:'@+               
       ,'@@@'....:'+';;+;,,,,,,,,,,,,:'''@@'               
      ,;;@@@+,.,@'#@@@@@@@@#;:..,:@@@@@@@@@+               
     ,;:::@@@#'@'@@@@@@@@@@@@##+@@@@@@@@@@@@               
     ,::,.:@#@@';,:+#@@@@@@@@@'+@@@@@@@@@@@@               
     .,:..:+:;+@,.;,:'+@@@@@@@+@@@@@@@@@@@@@`              
     `,,.,:;,,.#,',:'#@@@@@@@@+@@@@@@@@@@@@@.              
      ...:,,...+.@@@@@@@@@@@@#;@@@@@@@@@@@@@:              
      .,,....,.+@@@@@@@@@@@@@;.'@@@@@@@@@@@@;              
      .,;..:.,,,@@@@@@@@@@@@@..+@@@@@@@@@@@@:              
      `,.,.,,,,,'@@@@@@@@@#+...:@@@@@@@@@@@@`              
      `...,;,,,,.;@@@@@@@@#...,:#@@@@@@@@@@@               
      `,..,:,,,....#@@@@@;.....:+#@@@@@@@@@#               
       :.,:',,......,,,,,,.....:'++@@@@@#:@,               
       +,::;:,........,,,,.....,'';;;;:,,:@                
       .;:;;:,........,,,.....,,:;'::,,,,:@                
        ;+;;:.........,,.......,;;;:,,,,,+@                
        .''':,.......,,...,:..,';:;:,,,,,@@.               
         +';:,......,,.,,:':;,;@#;:,,,,,,@@#               
         '''::........,:''+++++@@+;:,,:,:'@@#              
         '+'::,......:;;;''';#;@@###;::::;'@@@+.           
         ;'+:,,.....:;';;::,,;,;@@@#+;::+'@#'#@@@,         
         ''+',,,...:;'':,,,,,,:;;'#@+':;@@#@++@#@@;        
         #+'+:,:,,::'';;:,::,:''++'++';#@@++#+'+#@@@`      
        ;@+;+;::::::;:,..,.,,:;++@+'''+#@@@@@@#''@@@@.     
       .@#':;+;:;::;::,....,,:;;;';''+@##+++#;;';'@@@##.   
      ,@@#;:,#+'';';;,....:::;'';;;'##@@#+++';';'+++;;;@@; 
    .#@@#':,,,@#++'';,....;'+++'';'#@@@@#+#'';'+++';;:;+@@@
   .@####;,,..;@#++'':..,,,;++';;;+@@@@@@++''''#+';;:;;'#''
   ##++#':,....+@##+',.,,,::;';;;'#@@@@###+'''###';:;;''';;
  +#+'++:,......####+:,,,::;;;'''+@@@@@++#+'+++#+;:;;;;;;;:
 .#+'+';,,......,#@#+;;::::;;''++#@@@@@@#++++##+;;;;;;;:;::
`#+;+'',,,.......,+@@+''';;;''+##@@@@'@@##+#+#+';;;;;;:;;::
+#;'';':,,........,;@@+#'''++##@@@@@'+@@+#++##+;;;;;;:;:'::
+#'';:+;,,.........,,@@@@@@@@@@@@@@''+#@+'#+++;;;;;;:;::':;
@#+'':'+,,...........,:@@@@@@@@@@@'''##+'+##+';;;;;:;:::+;:
@@+';;++,.............,,:+@@@@@@+'''++#''++#+;;;;;;;::::;+;
@@#'+;#+;,...........,.,,,:;''''''''+#+''#++;;;;;;;::::,:@;";
    }
}
