using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RenameFiles
{
    internal class Program
    {
        private const string SERIES_REGEX = @"(_UNPACK_)?(?<SHOW>.+)(?<SEASON>[sS](?<SEASONNUMBER>\d+)[eE]\d+)\.?((?<DESCRIPTION>.*)(?<QUALITY>(720|1080)p?))?";
        private static string logFilePath;

        private static string[] testnames = {@"Young.Sheldon.S02E05.1080p.HDTV.x264-CRAVERS-postbot.1",
"young.sheldon.s02e22.internal.720p.web.x264-bamboozle-BUYMORE.1",
"Young.Sheldon.S01E15.Dolomite.Apple.Slices.and.a.Mystery.Woman.1080p.WEB-DL.DD5.1.H.264-YFN-Obfuscated.1",
"RuPauls.Drag.Race.Untucked.S04E05.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S04E06.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S04E07.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S04E08.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S04E09.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S04E01.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E02.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E01.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E03.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E04.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E05.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E06.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E07.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E08.Life.Liberty.and.the.Pursuit.of.Style.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E09.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E10.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E11.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S03E12.1080p.WEBRip-AKU-Obfuscated",
"RuPauls.Drag.Race.Untucked.S02E01.1080p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.Untucked.S02E03.1080p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.Untucked.S02E02.1080p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.Untucked.S02E04.1080p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.Untucked.S02E05.1080p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.Untucked.S02E07.1080p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.Untucked.S02E08.1080p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.Untucked.S02E06.1080p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.Untucked.S02E09.1080p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.All.Stars.S05E01.REPACK.1080p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.All.Stars.S05E02.720p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.All.Stars.S05E03.720p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.All.Stars.S05E04.720p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.All.Stars.S05E05.720p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.All.Stars.S05E06.720p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.All.Stars.S05E07.720p.WEB.h264-SECRETOS",
"RuPauls.Drag.Race.All.Stars.S04E01.1080p.WEB-DL.AAC2.0.H.264.1-Fabutrash-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S04E02.720p.WEB.x264-SECRETOS-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S04E03.720p.WEB.x264-SECRETOS-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S04E04.720p.WEB.x264-SECRETOS-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S04E05.720p.WEB.x264-SECRETOS-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S04E06.720p.WEB.x264-SECRETOS-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S04E07.720p.WEB.x264-SECRETOS-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S04E08.720p.WEB.x264-SECRETOS-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S04E09.720p.WEB.x264-SECRETOS-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S04E10.1080p.WEB.x264.1-SECRETOS-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S03E01.1080p.VH1.WEBRip.AAC2.0.x264-BTN-postbot",
"RuPauls.Drag.Race.All.Stars.S03E02.Divas.Lip.Sync.Live.720p.VH1.WEB-DL.AAC2.0.x264.1-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S03E03.The.B tchelor.720p.VH1.WEB-DL.AAC2.0.x264.1-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S03E04.Snatch.Game.1080p.VH1.WEB-DL.AAC2.0.x264.1-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S03E05.1080p.VH1.WEBRip.AAC2.0.x264-BTN-postbot",
"RuPauls.Drag.Race.All.Stars.S03E06.Handmaids.to.Kitty.Girls.720p.VH1.WEB-DL.AAC2.0.x264.1-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S03E07.My.Best.Squirrelfriends.Dragsmaids.Wedding.Trip.1080p.VH1.WEB-DL.AAC2.0.x264.1-Obfuscated",
"RuPauls.Drag.Race.All.Stars.S03E08.A.Jury.of.Their.Queers.1080p.VH1.WEB-DL.AAC2.0.x264.1-Obfuscated",
"_UNPACK_RuPauls.Drag.Race.All.Stars.S02E03.1080p.WEB-DL.AAC2.0.H.264-RTN",
"RuPauls.Drag.Race.All.Stars.S01E01.1080p.WEB.h264-SECRETOS",
"_UNPACK_RuPauls.Drag.Race.All.Stars.S01E05.1080p.WEB.h264-SECRETOS",
"Young.Sheldon.S03E18.iNTERNAL.720p.WEB.H264-AMRAP.1",
"Young.Sheldon.S02E21.A.Broken.Heart.and.a.Crock.Monster.REPACK.720p.AMZN.WEB-DL.DDP5.1.H.264-NTb-BUYMORE.1",
"Young.Sheldon.S01E21.Summer.Sausage.a.Pocket.Poncho.and.Tony.Danza.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated.1",
"Young.Sheldon.S01E20.A.Dog.a.Squirrel.and.a.Fish.Named.Fish.1080p.WEB-DL.DD5.1.H.264-YFN-Scrambled.1",
"Young.Sheldon.S01E19.Gluons.Guacamole.and.the.Color.Purple.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated.1",
"Young.Sheldon.S01E17.Jiu-jitsu.Bubble.Wrap.and.Yoo-hoo.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated.1",
"Young.Sheldon.S01E14.1080p.WEB.x264-TBS-Obfuscated.1",
"Young.Sheldon.S01E12.A.Computer.a.Plastic.Pony.and.a.Case.of.Beer.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated.1",
"Young.Sheldon.S01E13.A.Sneeze.Detention.and.Sissy.Spacek.1080p.WEB-DL.DD5.1.H.264-YFN-Scrambled.1",
"Young.Sheldon.S01E11.Demons.Sunday.School.and.Prime.Numbers.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated.1",
"_UNPACK_Monsters.Inside.Me.S08E04.The.Monster.In.My.Mouth.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S08E02.My.Brain.Is.Under.Attack.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"_UNPACK_Monsters.Inside.Me.S07E03.Theres.Something.Living.In.My.Hand.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"_UNPACK_Monsters.Inside.Me.S06E10.They.Hijacked.My.Eyeball.720p.WEB.x264-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S06E09.All.I.Got.For.Christmas.Is.Brain.Surgery.720p.HDTV.x264-DHD",
"Monsters.Inside.Me.S06E08.The.Backyard.Killer.720p.HDTV.x264-DHD",
"Monsters.Inside.Me.S06E05.Worms.Are.Eating.My.Lungs.720p.HDTV.x264-DHD",
"Monsters.Inside.Me.S06E01.Theres.Something.Living.In.My.Knee.720p.HDTV.x264-DHD",
"Monsters.Inside.Me.S05E03.A.Menace.in.My.Own.Backyard.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"_UNPACK_Monsters.Inside.Me.S05E02.West.Nile.Attack.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S05E02.West.Nile.Attack.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S05E08.My.Body.Is.Rotting.720p.HDTV.x264-DHD.1",
"Monsters.Inside.Me.S05E07.A.Holiday.In.The.Hospital.720p.HDTV.x264-DHD",
"Monsters.Inside.Me.S05E06.Vampire.Parasites.Attack.720p.HDTV.x264-DHD.1",
"Monsters.Inside.Me.S05E05.Theres.A.Worm.Crawling.In.My.What.720p.HDTV.x264-DHD-Obfuscated",
"Monsters.Inside.Me.S05E04.The.Killer.In.The.Lake.720p.HDTV.x264-DHD",
"Monsters.Inside.Me.S05E01.My.Daughters.Going.Crazy.720p.HDTV.x264-DHD-Obfuscated",
"Monsters.Inside.Me.S05E08.My.Body.Is.Rotting.720p.HDTV.x264-DHD",
"Monsters.Inside.Me.S05E06.Vampire.Parasites.Attack.720p.HDTV.x264-DHD",
"Monsters.Inside.Me.S04E10.I.Almost.Killed.My.Baby.720p.WEB.x264-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S04E02.Theres.a.Worm.in.My.Eye.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S04E01.The.Flesh-eating.Monster.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S04E09.My.Christmas.from.Hell.720p.WEB.x264-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S04E06.Dying.Abroad.720p.WEB.x264-CAFFEiNE-Obfuscated",
"_UNPACK_Monsters.Inside.Me.S04E08.A.Deadly.Swim.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S04E05.My.Husband.is.Hallucinating.720p.WEB.x264-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S04E08.A.Deadly.Swim.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S04E04.It.Came.from.a.Tick.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S04E03.Choosing.Between.Life.and.Limb.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Monsters.Inside.Me.S03E08.My.Brain.Has.Been.Hijacked.720p.HDTV.DD5.1.x264",
"_UNPACK_Monsters.Inside.Me.s03e02.Something.Is.Eating.My.Son.Inside.Out.720p.HDTV.x264",
"Monsters.Inside.Me.s03e02.Something.Is.Eating.My.Son.Inside.Out.720p.HDTV.x264",
"Monsters.Inside.Me.S03E07.I.Coughed.Up.Worms.720p.HDTV.DD5.1.x264",
"Monsters.Inside.Me.S03E01.My.Child.Will.Only.Eat.Cat.Food.720p.HDTV.DD5.1.x264.1",
"Monsters.Inside.Me.s03e06.A.Monsters.Taking.My.Baby.720p.HDTV.x264",
"Monsters.Inside.Me.S03E05.Somethings.Eating.My.Dreams.720p.HDTV.DD5.1.x264",
"Monsters.Inside.Me.S03E04.My.Daughter.is.Losing.Her.Mind.720p.HDTV.DD5.1.x264",
"Monsters.Inside.Me.s03e03.My.Face.Eating.Parasite.720p.HDTV.x264",
"_UNPACK_Monsters.Inside.Me.S03E01.My.Child.Will.Only.Eat.Cat.Food.720p.HDTV.DD5.1.x264",
"Monsters.Inside.Me.S02E10.HDTV.XviD-OMiCRON",
"Monsters.Inside.Me.s02e08.Double.Agents.720p.HDTV.x264",
"Monsters.Inside.Me.S02E07.Breeders.720p.HDTV.DD5.1.x264",
"Monsters.Inside.Me.S02E05.Flesh.Eaters.720p.HDTV.DD5.1.x264",
"Monsters.Inside.Me.S02E04.Lurkers.720p.HDTV.DD5.1.x264",
"Monsters.Inside.Me.S02E09.Homegrown.Enemies.720p.HDTV.DD5.1.x264",
"Monsters.Inside.Me.S02E03.Cold.Blooded.Killers.720p.HDTV.DD5.1.x264",
"Monsters.Inside.Me.S01E06.Living.With.The.Enemy.720p.HDTV.x264",
"Monsters.Inside.Me.S01E05.Hijackers.720p.HDTV.x264",
"Monsters Inside Me S05E10 I Smell Like Death 720p HDTV x264-DHD",
"Monsters Inside Me S05E09 The Brain Colonizer 720p HDTV x264-DHD",
"_UNPACK_Mom.S03E09.720p.HDTV.X264-DIMENSION",
"_UNPACK_Lucifer.S04E05.1080p.NF.WEB-DL.DDP5.1.x264.1-NTb-Obfuscated",
"History.Channel.The.Universe.S05E08.Dark.Future.Of.The.Sun.720p.HDTV.x264-DHD",
"History.Channel.The.Universe.S05E07.Total.Eclipse.720p.HDTV.x264-DHD",
"History.Channel.The.Universe.S05E06.Asteroid.Attack.720p.HDTV.x264-DHD",
"Mom.S05E08.An.Epi-pen.and.a.Security.Cat.720p.AMZN.WEBRip.DDP5.1.x264.1-NTb-Obfuscated",
"Mom.S05E22.Diamond.Earrings.and.a.Pumpkin.Head.1080p.AMZN.WEB-DL.DDP5.1.H.264-NTb-Rakuv",
"mom.s06e03.1080p.web.x264-cookiemonster-BUYMORE",
"Mom.S07E04.iNTERNAL.1080p.WEB.x264.1-BAMBOOZLE-Obfuscated",
"mom.s06e02.1080p.web.x264-tbs-BUYMORE",
"mom.s05e21.1080p.web.x264-tbs-Rakuv",
"Mom.S07E03.iNTERNAL.1080p.WEB.x264.1-BAMBOOZLE-Obfuscated",
"Mom.S06E01.iNTERNAL.1080p.WEB.H264-METCON-postbot",
"mom.s05e20.1080p.web.x264-tbs",
"Mom.S07E02.iNTERNAL.1080p.WEB.x264-BAMBOOZLE-Obfuscated",
"mom.s05e19.1080p.web.x264-tbs-BUYMORE",
"Mom.S07E01.iNTERNAL.1080p.WEB.H264.1-AMRAP-Obfuscated",
"mom.s05e18.internal.1080p.web.x264-bamboozle-BUYMORE",
"mom.s05e17.1080p.web.x264-tbs-BUYMORE",
"mom.s05e16.internal.1080p.web.x264-bamboozle",
"mom.s05e15.1080p.web.x264-tbs-BUYMORE",
"mom.s05e14.1080p.web.x264-tbs-BUYMORE",
"mom.s05e13.1080p.web.x264-tbs-BUYMORE",
"Mom.S05E12.1080p.CBS.WEB-DL.AAC2.0.H.264-PyR8zdl-BUYMORE",
"Lucifer.S03E08.720p.HDTV.x264-AVS-WhiteRev-Obfuscated.1",
"Mom.S05E11.1080p.WEB.x264.1-TBS-Obfuscated",
"Lucifer.S03E07.720p.HDTV.x264-AVS-WhiteRev-Obfuscated.1",
"mom.s05e03.1080p.web.x264-convoy-BUYMORE",
"Lucifer.S03E06.720p.HDTV.x264-KILLERS-WhiteRev-Obfuscated.1",
"mom.s05e02.1080p.web.x264-tbs-BUYMORE",
"Lucifer.S03E08.720p.HDTV.x264-AVS-WhiteRev-Obfuscated",
"Mom.S05E01.Twinkle.Lights.and.Grandma.Shoes.1080p.AMZN.WEB-DL.DDP5.1.H.264-NTb-BUYMORE",
"Lucifer.S03E04.720p.HDTV.X264-DIMENSION-WhiteRev.1",
"Lucifer.S03E07.720p.HDTV.x264-AVS-WhiteRev-Obfuscated",
"Lucifer.S03E03.720p.HDTV.X264-DIMENSION-WhiteRev.1",
"Lucifer.S03E06.720p.HDTV.x264-KILLERS-WhiteRev-Obfuscated",
"_UNPACK_Lucifer.S03E11.City.of.Angels.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Rakuv.1",
"Lucifer.S03E11.City.of.Angels.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Rakuv.1",
"Lucifer.S03E04.720p.HDTV.X264-DIMENSION-WhiteRev",
"Lucifer.S03E10.The.Sin.Bin.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Scrambled.1",
"Lucifer.S03E03.720p.HDTV.X264-DIMENSION-WhiteRev",
"Lucifer.S03E09.The.Sinnerman.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Rakuv.1",
"Lucifer.S03E11.City.of.Angels.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Rakuv",
"_UNPACK_Lucifer.S03E05.Welcome.Back.Charlotte.Richards.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Rakuv.1",
"Lucifer.S03E05.Welcome.Back.Charlotte.Richards.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Rakuv.1",
"Lucifer.S03E10.The.Sin.Bin.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Scrambled",
"Lucifer.S03E02.The.One.With.The.Baby.Carrot.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Rakuv.1",
"Lucifer.S03E09.The.Sinnerman.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Rakuv",
"lucifer.s03e25.1080p.web.x264-tbs-Rakuv.1",
"Lucifer.S03E05.Welcome.Back.Charlotte.Richards.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Rakuv",
"Lucifer.S03E24.1080p.WEB.x264-TBS-postbot.1",
"Lucifer.S03E02.The.One.With.The.Baby.Carrot.720p.AMZN.WEBRip.DDP5.1.x264-NTb-Rakuv",
"lucifer.s03e23.1080p.web.x264-cookiemonster.1",
"lucifer.s03e25.1080p.web.x264-tbs-Rakuv",
"Lucifer.S03E24.1080p.WEB.x264-TBS-postbot",
"Lucifer.S03E21.1080p.WEB.x264.1-TBS-Obfuscated.1",
"lucifer.s03e23.1080p.web.x264-cookiemonster",
"Lucifer.S03E20.INTERNAL.1080p.WEB.H264.1-DEFLATE-Obfuscated.1",
"Lucifer.S03E21.1080p.WEB.x264.1-TBS-Obfuscated",
"Lucifer.S03E20.INTERNAL.1080p.WEB.H264.1-DEFLATE-Obfuscated",
"Lucifer.S03E17.1080p.WEB.x264.1-TBS-Obfuscated.1",
"Lucifer.S03E16.1080p.WEB.x264-TBS-postbot.1",
"Lucifer.S03E15.1080p.WEB.x264.1-TBS-Obfuscated.1",
"Lucifer.S03E17.1080p.WEB.x264.1-TBS-Obfuscated",
"Lucifer.S03E14.1080p.WEB.x264-TBS-postbot.1",
"Lucifer.S03E16.1080p.WEB.x264-TBS-postbot",
"Lucifer.S03E13.1080p.WEB.x264-TBS-postbot.1",
"Lucifer.S03E15.1080p.WEB.x264.1-TBS-Obfuscated",
"Lucifer.S03E12.1080p.WEB.x264.1-TBS-Obfuscated.1",
"Lucifer.S03E14.1080p.WEB.x264-TBS-postbot",
"Lucifer.S03E13.1080p.WEB.x264-TBS-postbot",
"Lucifer.S03E12.1080p.WEB.x264.1-TBS-Obfuscated",
"Lucifer.S02E10.1080p.WEB-DL-Obfuscated",
"Lucifer S02E07 1080p WEB-DL-Obfuscated",
"Lucifer.S02E06.1080p.WEB-DL.DD5.1.H.264-DRACULA",
"The.Haunting.Of.S05E09.Morgan.Fairchild.720p.HDTV.x264-DHD-Obfuscated.1",
"The.Haunting.Of.S04.Revealed.Special.720p.HDTV.x264-DHD",
"The.Haunting.Of.S05E08.Ernie.Hudson.720p.HDTV.x264-DHD.1",
"The.Haunting.Of.S04.Scariest.Spirits.Special.720p.HDTV.x264-DHD",
"The.Haunting.Of.S05E07.Tionne.Watkins.720p.HDTV.x264-DHD-Obfuscated.1",
"The Haunting Of S04 Creepiest Locations Special 720p HDTV x264-DHD",
"The.Haunting.Of.S05E06.Eric.Balfour.720p.HDTV.x264-DHD.1",
"The.Haunting.Of.S03E04.Sally.Struthers.HDTV.XviD-AFG",
"The.Haunted.S02E09.House.of.the.Rising.Dead.DSR.x264-W4F",
"The.Haunted.S02E06.Murder.in.Room.12.720p.HDTV.x264-DHD",
"The.Haunted.S02E05.Stalked.By.a.Vampire.720p.HDTV.x264-DHD",
"the.haunted.s02e04.720p-dhd",
"How.the.Universe.Works.S05E08.Strangest.Alien.Worlds.1080p.WEB.x264.1-CRiMSON-Obfuscated",
"How.The.Universe.Works.S03.1080p.AMZN.WEB-DL.DD.2.0.H.264.1-SiGMA-Obfuscated.2",
"A.Haunting.S10E10.Gateway.to.Evil.720p.WEBRip.x264-CAFFEiNE-Obfuscated",
"A.Haunting.S10E09.When.the.Lights.Go.Out.720p.WEBRip.x264-CAFFEiNE-Obfuscated",
"A.Haunting.S09E12.Untouchable.iNTERNAL.720p.HDTV.x264-DHD",
"A.Haunting.S09E10.Bewitched.iNTERNAL.720p.HDTV.x264-DHD-Obfuscated",
"A.Haunting.S09E09.720p.HDTV.x264-W4F",
"A.Haunting.S09E07.Mothers.Terror.720p.HDTV.x264-W4F-Obfuscated",
"A.Haunting.S09E06.Fear.Feeders.720p.HDTV.x264-W4F-Obfuscated.1",
"A.Haunting.S09E06.Fear.Feeders.720p.HDTV.x264-W4F-Obfuscated",
"A.Haunting.S09E05.Ghost.Protector.720p.HDTV.x264-W4F",
"A.Haunting.S05E10.Deaths.Door.720p.HDTV.x264-DHD",
"A.Haunting.S04E13.RERiP.iNTERNAL.BDRip.x264-LiBRARiANS",
"Paranormal.Witness.S02E07.The.Real.Haunting.in.Connecticut.720p.HDTV.x264-DHD.1",
"Paranormal.Witness.S02E07.The.Real.Haunting.in.Connecticut.720p.HDTV.x264-DHD",
"_UNPACK_Drain.the.Oceans.S02E03.Killer.U-Boats.720p.WEBRip.x264-CAFFEiNE-Obfuscated",
"Drain.the.Oceans.S01E00.Villains.of.the.Underworld.720p.HDTV.x264-DHD-Obfuscated.1",
"Drain.the.Oceans.Secrets.of.the.Civil.War.720p.WEBRip.x264-CAFFEiNE.1",
"Young.Sheldon.S01E03.1080p.WEB.x264-TBS-Obfuscated",
"Young.Sheldon.S01E02.Rockets.Communists.and.the.Dewey.Decimal.System.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated",
"Young.Sheldon.S01E05.A.Solar.Calculator.a.Game.Ball.and.a.Cheerleaders.Bosom.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated",
"Young.Sheldon.S01E06.1080p.WEB.x264-TBS-Obfuscated",
"Young.Sheldon.S01E08.Cape.Canaveral.Schrodingers.Cat.and.Cyndi.Laupers.Hair.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated",
"Young.Sheldon.S01E09.Spock.Kirk.and.Testicular.Hernia.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated",
"Young.Sheldon.S03E18.iNTERNAL.720p.WEB.H264-AMRAP",
"Young.Sheldon.S02E21.A.Broken.Heart.and.a.Crock.Monster.REPACK.720p.AMZN.WEB-DL.DDP5.1.H.264-NTb-BUYMORE",
"Young.Sheldon.S01E21.Summer.Sausage.a.Pocket.Poncho.and.Tony.Danza.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated",
"Young.Sheldon.S01E20.A.Dog.a.Squirrel.and.a.Fish.Named.Fish.1080p.WEB-DL.DD5.1.H.264-YFN-Scrambled",
"Young.Sheldon.S01E19.Gluons.Guacamole.and.the.Color.Purple.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated",
"Young.Sheldon.S01E17.Jiu-jitsu.Bubble.Wrap.and.Yoo-hoo.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated",
"Young.Sheldon.S01E14.1080p.WEB.x264-TBS-Obfuscated",
"Young.Sheldon.S01E15.Dolomite.Apple.Slices.and.a.Mystery.Woman.1080p.WEB-DL.DD5.1.H.264-YFN-Obfuscated",
"Young.Sheldon.S01E13.A.Sneeze.Detention.and.Sissy.Spacek.1080p.WEB-DL.DD5.1.H.264-YFN-Scrambled",
"Young.Sheldon.S01E12.A.Computer.a.Plastic.Pony.and.a.Case.of.Beer.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated",
"Young.Sheldon.S01E11.Demons.Sunday.School.and.Prime.Numbers.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated",
"Young.Sheldon.S01E10.An.Eagle.Feather.a.String.Bean.and.an.Eskimo.1080p.WEB-DL.DD5.1.H.264.1-YFN-Obfuscated",
"Strange.Evidence.S04E03.When.Bigfoot.Attacks.1080p.WEB.h264-ROBOTS",
"Monsters.and.Mysteries.in.America.S01E01.Appalachia.720p.HDTV.x264-DHD",
"Strange.Evidence.S03E10.Curse.of.Poltergeist.Hill.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S03E09.It.Came.From.the.Fog.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S03E08.Alien.in.the.Abyss.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S03E07.Return.of.the.Witch.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S03E06.The.Skinwalker.Awakens.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S03E05.Doomsday.Volcano.NYC.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S03E04.Secrets.of.the.Himalaya.Alien.1080p.WEB.x264.1-UNDERBELLY-Obfuscated",
"Strange.Evidence.S02E06.The.Beast.That.Ate.Jaws.720p.WEB.x264-CAFFEiNE-Obfuscated",
"Strange.Evidence.S03E03.The.Devils.Mutant.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S02E12.Midnight.Train.to.Hell.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S03E02.Stargate.in.the.Jungle.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S01E10.Vampire.Down.Under.1080p.WEB.x264.1-CRiMSON-Obfuscated",
"Strange.Evidence.S02E11.The.Incident.at.Area.51.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S03E01.Alien.Armageddon.Conspiracy.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S01E09.Ghost.in.the.Abyss.1080p.WEB.x264.1-CRiMSON-Obfuscated",
"Strange.Evidence.S02E10.Curse.of.the.Monster.Spider.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S02E09.Wrath.of.Thor.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S01E08.Lake.of.the.Dead.1080p.WEB.x264.1-CRiMSON-Obfuscated",
"Strange.Evidence.S02E08.The.Omen.of.Blood.River.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"_UNPACK_Secrets.of.the.Underground.S02E01.Legend.of.the.Nazi.Gold.1080p.WEB.x264.1-UNDERBELLY-Obfuscated",
"Strange.Evidence.S01E07.Jungle.Werewolf.1080p.WEB.x264.1-CRiMSON-Obfuscated",
"Strange.Evidence.S02E07.Doomsday.at.Yellowstone.1080p.WEB.x264.1-CAFFEiNE-Obfuscated",
"Strange.Evidence.S01E02.Attack.of.the.Fire.Devil.1080p.WEB.x264.1-CRiMSON-Obfuscated",
"Secrets.of.the.Underground.S01E03.iNTERNAL.720p.HDTV.x264-DHD-Obfuscated",
"_UNPACK_Monsters.and.Mysteries.in.America.S03E08.Mantis.Man.720p.HDTV.x264-DHD-Obfuscated",
"Monsters.and.Mysteries.in.America.S03E06.Mill.Race.Monster.720p.HDTV.x264-DHD",
"Monsters.and.Mysteries.in.America.S03E05.Vermont.Pigman.720p.HDTV.x264-DHD",
"Monsters.and.Mysteries.in.America.S03E02.Tennesse.Wildman.720p.HDTV.x264-DHD",
"Monsters.and.Mysteries.in.America.S03E01.Men.in.Black.720p.HDTV.x264-DHD",
"Ghost.Inside.My.Child.S02E12.Drowned.at.Sea.and.First.Degree.720p.HDTV.x264-DHD",
"Monsters.and.Mysteries.in.America.S01E05.The.Swamp.720p.HDTV.x264-DHD",
"Monsters.and.Mysteries.in.America.S01E03.Ozarks.720p.HDTV.x264-DHD",
"How.The.Universe.Works.S03.1080p.AMZN.WEB-DL.DD.2.0.H.264.1-SiGMA-Obfuscated.1",
"_UNPACK_Ghost.Inside.My.Child.S02E07.Family.Drama.and.Military.Trauma.720p.HDTV.x264-DHD",
"Forbidden.History.S02E01.720p.HDTV.x264-TViLLAGE",
"Drain.the.Oceans.S01E01.Nazi.Secrets.720p.HDTV.x264-DHD-Obfuscated",
"Drain.the.Oceans.S01E00.Villains.of.the.Underworld.720p.HDTV.x264-DHD-Obfuscated",
"Drain.the.Oceans.Secrets.of.the.Civil.War.720p.WEBRip.x264-CAFFEiNE",
"The.Haunting.Of.S04E07.Michael.Madsen.720p.HDTV.x264-DHD",
"The.Haunting.Of.S04E32.Karina.Smirnoff.720p.HDTV.x264-DHD",
"The.Haunting.Of.S04E33.Meat.Loaf.720p.HDTV.x264-DHD",
"The.Haunting.Of.S04E34.Melody.Thomas.Scott.720p.HDTV.x264-DHD",
"The.Haunting.Of.S05E06.Eric.Balfour.720p.HDTV.x264-DHD",
"The.Haunting.Of.S05E07.Tionne.Watkins.720p.HDTV.x264-DHD-Obfuscated",
"The.Haunting.Of.S05E08.Ernie.Hudson.720p.HDTV.x264-DHD",
"The.Haunting.Of.S05E09.Morgan.Fairchild.720p.HDTV.x264-DHD-Obfuscated",
"How.The.Universe.Works.S03.1080p.AMZN.WEB-DL.DD.2.0.H.264.1-SiGMA-Obfuscated",
"_UNPACK_Ancient.Aliens.S15E04.1080p.WEB.h264-TRUMP",
"Young.Sheldon.S02E15.iNTERNAL.720p.WEB.x264-BAMBOOZLE-postbot",
"Young.Sheldon.S02E09.HDTV.x264-SVA-postbot",
"Young.Sheldon.S02E08.An.8-Bit.Princess.and.a.Flat.Tire.Genius.720p.AMZN.WEBRip.DDP5.1.x264-NTb-postbot",
"Young.Sheldon.S02E15.iNTERNAL.720p.WEB.x264-BAMBOOZLE-postbot.1",
"Young.Sheldon.S02E07.Carbon.Dating.and.a.Stuffed.Raccoon.720p.AMZN.WEBRip.DDP5.1.x264-NTb-postbot.1",
"Young.Sheldon.S02E08.An.8-Bit.Princess.and.a.Flat.Tire.Genius.720p.AMZN.WEBRip.DDP5.1.x264-NTb-postbot.1",
"Young.Sheldon.S02E09.HDTV.x264-SVA-postbot.1",
"Young.Sheldon.S02E07.Carbon.Dating.and.a.Stuffed.Raccoon.720p.AMZN.WEBRip.DDP5.1.x264-NTb-postbot",
"Young.Sheldon.S02E06.Seven.Deadly.Sins.and.a.Small.Carl.Sagan.720p.AMZN.WEBRip.DDP5.1.x264-NTb-postbot",
"Young.Sheldon.S02E04.A.Financial.Secret.and.Fish.Sauce.720p.AMZN.WEB-DL.DDP5.1.H.264-NTb-RakuvFIN",
"young.sheldon.s02e22.internal.720p.web.x264-bamboozle-BUYMORE" };

        private static void Main(string[] args)
        {
            //string[] allfiles = Directory.GetFiles("path/to/dir", "*.*", SearchOption.AllDirectories);

            // ... Use named group in regular expression.
            Regex expression = new Regex(SERIES_REGEX);
            Match match;

            logFilePath = $"{Directory.GetCurrentDirectory()}//rename_log_{DateTime.Now.ToString("yyMMdd_HHmm")}.txt";
            //File.Create(logFilePath);
            //File.

            var folders = Directory.GetDirectories(Directory.GetCurrentDirectory());
            foreach (string folder in folders)
            {
                Console.ResetColor();
                Console.WriteLine("");
                try
                {
                    string folderName = folder.Split('\\').Last();
                    Log($"Checking \"{ folderName }\"", ConsoleColor.Gray);
                    match = expression.Match(folderName);
                    if (!match.Success)
                    {
                        Log("Folder does not match name format", ConsoleColor.Red);
                    }
                    if (match.Success)
                    {
                        string[] files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly);
                        if (files.Length == 0)
                        {
                            Log($"Folder \"{ folder }\" is empty", ConsoleColor.Gray);
                            string emptyFolder = $"{folder}-EMPTY";
                            if (!Directory.Exists(emptyFolder))
                            {
                                Log($"Renaming folder \"{ folder }\"", ConsoleColor.Gray);
                                Directory.Move(folder, $"{folder}-EMPTY");

                                if (Directory.Exists(emptyFolder))
                                {
                                    Log($"Deleting folder \"{ folder }\"", ConsoleColor.Red);
                                    Directory.Delete(emptyFolder, true);
                                }
                                continue;
                            }
                        }

                        FileInfo fi = new FileInfo(files[0]);

                        string show = match.Groups["SHOW"].Value.Replace(".", " ");
                        string season = match.Groups["SEASON"].Value;
                        string seasonNumber = match.Groups["SEASONNUMBER"].Value;
                        string description = match.Groups["DESCRIPTION"].Value;
                        string quality = match.Groups["QUALITY"].Value;
                        string newName = $"{show.Replace(".", " ").Trim()} { season.Trim().ToUpper() } [{ quality.Trim() }]{fi.Extension}";

                        if (!string.IsNullOrEmpty(description.Trim()))
                        {
                            newName = $"{show.Replace(".", " ").Trim() } { season.Trim().ToUpper() } - { description.Replace(".", " ").Trim() } [{ quality.Trim() }]{fi.Extension}";
                        }

                        string newPath = GetPathForSeries(show, int.Parse(seasonNumber));
                        string currentPath = fi.FullName;
                        string newPathAndName = $"{newPath}\\{newName}";

                        Log($"Creating file \"{ newName }\" in \"{newPath}\"", ConsoleColor.Cyan);

                        if (File.Exists(newPathAndName))
                        {
                            Log($"File \"{newName}\" already exists in \"{newPathAndName}\"!", ConsoleColor.Red);
                            continue;
                        }

                        File.Copy(currentPath, newPathAndName);

                        if (File.Exists(newPathAndName))
                        {
                            Log($"Deleting folder \"{ folderName }\"", ConsoleColor.Yellow);

                            Directory.Delete(folder, true);

                            Log("Success", ConsoleColor.Green);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message, ConsoleColor.Red);
                    Console.WriteLine("");
                }

                LogEmptyLine();
            }

            Console.ReadLine();
        }

        private static string GetPathForSeries(string seriesName, int seasonNumber)
        {
            var seriesPaths = ConfigurationManager.AppSettings["SeriesFolders"].Split(',');
            var orphanPath = ConfigurationManager.AppSettings["OrphanFolder"];
            string seasonName = $"Season {seasonNumber}";
            string path;
            foreach (string seriesPath in seriesPaths)
            {
                path = $"{seriesPath}\\{seriesName.Trim()}";
                if (Directory.Exists(path))
                {
                    path = $"{path}\\{seasonName}";
                    if (!Directory.Exists(path))
                    {
                        if (CreateFolderIfNotExist(path))
                        {
                            return path;
                        }
                    }

                    return path;
                }
                else
                {
                    orphanPath = $"{orphanPath}\\{seriesName.Trim()}\\{seasonName}";
                    Directory.CreateDirectory(orphanPath);
                }
            }

            return orphanPath;
        }

        private static bool CreateFolderIfNotExist(string folderPath)
        {
            var pathParts = folderPath.Split('\\');
            List<string> parts = new List<string>();
            StringBuilder pathChecked = new StringBuilder();
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);

                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        private static void Log(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            StringBuilder sb = new StringBuilder();
            sb.Append(text);

            // flush every 20 seconds as you do it
            //File.AppendAllText(logFilePath, $"{DateTime.Now}\t{sb.ToString()}");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logFilePath, true))
            {
                file.WriteLine($"{DateTime.Now}\t{sb.ToString()}");
            }

            sb.Clear();

            Console.ResetColor();
        }

        private static void LogEmptyLine()
        {
            Console.WriteLine("");
            StringBuilder sb = new StringBuilder();
            sb.Append("");

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(logFilePath, true))
            {
                file.WriteLine($"{DateTime.Now}\t{sb.ToString()}");
            }

            sb.Clear();
        }
    }
}