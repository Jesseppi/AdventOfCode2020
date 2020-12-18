﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var operations = GetDataChunks(BitMask.Commands);
            var value = GetValue(operations);

            Console.WriteLine(value);
        }

        public static long GetValue(Operations operations)
        {
            var memoryBlock = new Dictionary<int, long>();

            foreach (var operation in operations.OperationList)
            {
                foreach (var command in operation.Operations)
                {
                    var value = GetMaskedValue(command, operation.Mask);
                    if (memoryBlock.ContainsKey(value.Item1)) { memoryBlock[value.Item1] = value.Item2; }
                    else { memoryBlock.Add(value.Item1, value.Item2); }
                }
            }

            long sum = 0;
            foreach (var item in memoryBlock)
            {
                sum += item.Value;
            }

            return sum;
        }


        public static (int, long) GetMaskedValue((int, long) command, Dictionary<int, int> mask)
        {

            var binaryValue = Convert.ToString(command.Item2, 2);
            var paddedValue = binaryValue.PadLeft(36, '0').ToCharArray();

            foreach (var item in mask)
            {
                paddedValue[item.Key] = Convert.ToChar(item.Value.ToString());
            }

            var base36 = new string(paddedValue);
            var base2 = "1" + base36.Split("1", 2)[1];
            return (command.Item1, Convert.ToInt64(base2, 2));
        }

        public static Operations GetDataChunks(string data)
        {
            var operations = new Operations();

            var chunks = data.Split("mask = ");

            foreach (var chunk in chunks)
            {
                using var stringReader = new StringReader(chunk);

                var dataChunk = new DataChunk();

                var operation = string.Empty;
                string line = string.Empty;
                do
                {
                    line = stringReader.ReadLine();
                    if (line == null || string.IsNullOrWhiteSpace(line)) continue;
                    line = line.Trim();
                    if (!line.Contains("="))
                    {
                        if (!string.IsNullOrWhiteSpace(operation))
                        {
                            operation = string.Empty;
                            continue;
                        }
                        dataChunk.Mask = GetMask(line);
                        continue;
                    }
                    var memLocation = line.Split("=")[0].Split("[")[1].Split("]")[0];
                    var value = line.Split("=")[1].Trim();

                    dataChunk.Operations.Add((Int32.Parse(memLocation), Int64.Parse(value)));

                } while (line != null);
                operations.OperationList.Add(dataChunk);
            }

            return operations;

        }

        public static Dictionary<int, int> GetMask(string line)
        {
            var maskValues = new Dictionary<int, int>();
            var maskValue = line;

            for (var character = 0; character < maskValue.Length; character++)
            {
                if (maskValue[character] != 'X')
                {
                    var value = maskValue[character];
                    var numericalValue = Int32.Parse(value.ToString());
                    maskValues.Add(character, numericalValue);
                }
            }

            return maskValues;
        }

        public class Operations
        {
            public List<DataChunk> OperationList { get; set; } = new List<DataChunk>();
        }

        public class DataChunk
        {
            public Dictionary<int, int> Mask { get; set; }

            public List<(int, long)> Operations { get; set; } = new List<(int, long)>();

        }

        public static class BitMask
        {
            public static string Test =
                @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
                    mem[8] = 11
                    mem[7] = 101
                    mem[8] = 0";

            public static string Commands =
            @"mask = 1X110XX0101001X0110010X0X01001X1X101
                mem[42463] = 214519414
                mem[191] = 8220641
                mem[4479] = 894847027
                mem[37813] = 345
                mem[26369] = 11484356
                mask = 10X01101XX1101101011100X0001X1X10111
                mem[58048] = 880
                mem[24587] = 33469
                mem[64189] = 2241
                mem[23307] = 35703
                mask = 0XX011X1010101X0111100XXX00001010000
                mem[19250] = 36523
                mem[20565] = 228398867
                mem[36258] = 643831
                mem[22845] = 6717901
                mask = 1101111110111110X1011X0001001X10011X
                mem[44476] = 33279292
                mem[59128] = 1020
                mem[380] = 586
                mask = 111X11110X111X1011011X1X11X0101X10XX
                mem[1353] = 394122068
                mem[44615] = 22839985
                mem[35638] = 56308
                mask = 1X011XXX01111X0110110010010X111100XX
                mem[122] = 4859471
                mem[23073] = 4721114
                mem[2688] = 153
                mem[31838] = 8317163
                mem[34071] = 58
                mask = 100X011000101X0011X000X01010110000XX
                mem[25408] = 99847645
                mem[44176] = 15984473
                mem[3622] = 27559739
                mem[43628] = 9315248
                mem[41077] = 142316
                mask = 0011111X010X0110X101X011110100000000
                mem[40484] = 838027
                mem[15461] = 75031
                mem[9911] = 25956750
                mem[32721] = 58256
                mem[25503] = 31355454
                mask = 11011111101111101111X000X0X01X100011
                mem[13951] = 3187845
                mem[42611] = 499531374
                mem[1348] = 46027
                mask = X0101X0X0X110X1111X100X0111001101111
                mem[32721] = 14060
                mem[63438] = 776
                mem[20012] = 1324901
                mem[11372] = 19
                mask = 0011111XXX11XXX011010011011110010000
                mem[47120] = 104835008
                mem[11924] = 5172
                mem[45660] = 1431
                mask = X011111100XX0XX0XXX111011011101X0000
                mem[58959] = 228865
                mem[28674] = 1397
                mem[40066] = 22914331
                mem[35502] = 2214464
                mem[46257] = 38741
                mem[36434] = 238406
                mem[40704] = 793
                mask = 10111X111X11X110XX0111X0001000110100
                mem[49019] = 10888639
                mem[26568] = 10683033
                mem[17614] = 229505244
                mask = 100110111X11X10X1101X010000011010101
                mem[9920] = 126042390
                mem[38465] = 3661
                mask = 001X11010111X100X1X010010X101X111110
                mem[16284] = 15613
                mem[1960] = 4392972
                mem[8829] = 2152
                mem[18816] = 74
                mem[26350] = 3769
                mask = 1X01X11110X11110111100X00010101XX010
                mem[15461] = 1048143
                mem[24849] = 160700
                mem[44123] = 241991574
                mem[1959] = 267504879
                mem[60981] = 617
                mask = 100X101110X0111001010000010X1X010X01
                mem[1109] = 1771400
                mem[2386] = 391164
                mem[421] = 773104
                mask = 1X101101011001X01X1000010001001X1011
                mem[59385] = 175597895
                mem[44111] = 7916
                mem[29010] = 15365
                mem[56142] = 839
                mask = 0010X111011X0X10X10110110010X0XX1100
                mem[26355] = 2333647
                mem[11939] = 3268
                mem[36664] = 63076
                mem[58297] = 42760572
                mask = 1111101101001100110X01110X00001001XX
                mem[29521] = 443847244
                mem[17951] = 37377866
                mem[3240] = 6963715
                mem[7430] = 7084583
                mask = 1111X1110X110110X010000010X0X1111011
                mem[9102] = 1132
                mem[57131] = 594749
                mem[18471] = 1498
                mem[20612] = 18574
                mem[55991] = 29384
                mem[32097] = 6
                mask = 100X111101X1X1101X011011001100100000
                mem[30906] = 748336963
                mem[2756] = 2176516
                mask = 11011111011111X1XX1X1X1XX001011X1000
                mem[862] = 11244
                mem[47376] = 16422
                mem[46257] = 3350414
                mem[58048] = 4682099
                mask = X110X11X01111X101X01X011110X110010X0
                mem[40968] = 31617
                mem[193] = 43438781
                mem[26478] = 41624
                mem[5002] = 190295
                mem[65102] = 2394
                mem[58297] = 5080
                mask = 1110X1X10X10X111101X0X01000100101000
                mem[16494] = 9110
                mem[37930] = 8809
                mem[23593] = 481
                mask = 1X0100111010111X1X01000X011001010101
                mem[3126] = 97067
                mem[17775] = 4618893
                mem[28013] = 323275
                mem[1232] = 808595
                mask = 1110XX1101X0011X1011111X0001000X1110
                mem[21326] = 447393
                mem[3589] = 16728223
                mem[55002] = 28705304
                mem[51765] = 252696
                mem[21722] = 107935
                mem[58048] = 21916
                mem[57844] = 6641090
                mask = 100XXX1X1011110011011001000X11100100
                mem[40154] = 63503
                mem[30949] = 17478454
                mask = 00X111110001010010011X01111111001XX1
                mem[24685] = 225914
                mem[44224] = 11470806
                mask = 11XX111X0XX111011110X0X11XX111111001
                mem[47584] = 63259
                mem[19267] = 3951128
                mem[11954] = 108925154
                mem[13375] = 228945
                mask = 1010010101111110110110XX10X00XX11000
                mem[57820] = 127087966
                mem[53286] = 228525844
                mem[35915] = 2013
                mem[61517] = 314657
                mem[31045] = 1111881
                mask = 1110111X0X111010110111100100X0101X10
                mem[27505] = 581234653
                mem[36258] = 146176326
                mem[19968] = 2520
                mem[39878] = 5120239
                mem[35633] = 1012866
                mem[10668] = 1644
                mask = X0X11111101111100X11X1XX0X1010110110
                mem[1784] = 423
                mem[61032] = 650175
                mem[28870] = 311821
                mask = X10X1101011111X1X01100X0X10011X1X00X
                mem[65512] = 581295
                mem[8321] = 6021
                mem[47627] = 7130
                mem[40854] = 190200
                mem[4241] = 2706494
                mem[38697] = 1029
                mask = 1001XX11101X11X0X10100X001X01101010X
                mem[28013] = 14105
                mem[57161] = 117815
                mem[26369] = 1543
                mem[24827] = 485993415
                mem[1109] = 492187449
                mem[48047] = 33515342
                mask = 111011X10111011X101101X0001000X01011
                mem[39479] = 1938757
                mem[13170] = 178600656
                mem[19690] = 907312
                mem[62603] = 2277
                mem[11260] = 305416
                mask = 10XX01X010X0110011XX110010X101010100
                mem[122] = 76524853
                mem[25559] = 6103
                mask = X101111X0101XX101X011X10XX000X100101
                mem[48929] = 33148
                mem[40704] = 469591
                mem[41020] = 261
                mem[43738] = 25395
                mask = 1X10110101110X11X011000100X10011001X
                mem[38233] = 1730669
                mem[44780] = 927789
                mem[21353] = 39621682
                mem[56797] = 859037
                mem[16253] = 66164178
                mem[511] = 236279095
                mem[18782] = 3309223
                mask = 00X001X101100X100101XXX1X10000000111
                mem[28013] = 110434
                mem[15398] = 502727
                mem[10803] = 632
                mem[12743] = 160910
                mem[59385] = 562563
                mem[44558] = 3533
                mem[56352] = 4488
                mask = 11111XX10X1X0XX01101101X111101011000
                mem[23668] = 290432615
                mem[42648] = 22668784
                mem[8958] = 122482996
                mem[11890] = 111221412
                mem[47120] = 455
                mask = 0XXX011010111000XX11X111101X00000X00
                mem[26463] = 27088
                mem[15036] = 164314828
                mem[16062] = 11244
                mem[57407] = 1842472
                mem[34071] = 8262157
                mem[56074] = 1413102
                mem[58111] = 877
                mask = 1X111111011X1XX01101001X000011110100
                mem[38248] = 258523318
                mem[6993] = 260
                mem[19820] = 3961001
                mask = 10X11111XX1100000101X1X0X000011X1111
                mem[12868] = 1467
                mem[10699] = 25073
                mem[63473] = 3057
                mask = 11111111011X110011011010X1X00011X111
                mem[61032] = 637400
                mem[60557] = 122719
                mem[10373] = 36781
                mem[44798] = 696610765
                mem[29667] = 289872612
                mask = 100X10111XX11110010100X00X01X1010001
                mem[16253] = 100847
                mem[44558] = 495
                mem[10609] = 187
                mem[17877] = 1947882
                mem[7726] = 49
                mask = 1001X111X010111001X1XX00010011010000
                mem[29577] = 23392903
                mem[1348] = 217473
                mem[8213] = 894
                mask = XXX111110XX1011011011001X0100X1X000X
                mem[56275] = 33935422
                mem[14789] = 24610535
                mem[10036] = 3667417
                mem[34174] = 312390538
                mem[46449] = 71599
                mem[60711] = 1254846
                mem[56818] = 34757397
                mask = 101011010111111011001011XX01000111XX
                mem[32458] = 166252
                mem[39354] = 14250
                mem[59600] = 57000949
                mem[41225] = 21347
                mem[26478] = 6789
                mem[59493] = 412834892
                mask = 0X00011X1X1X11001X1111X1111111X01110
                mem[26410] = 477170
                mem[43186] = 24250562
                mem[38202] = 14846
                mem[14344] = 6848395
                mem[37708] = 3074
                mem[39298] = 2514
                mask = 1X0001100010XXX011101X001X1X11X00100
                mem[380] = 348672
                mem[12581] = 2854226
                mem[35376] = 3826235
                mem[46576] = 1704733
                mem[9973] = 87885
                mem[49981] = 371365297
                mask = 1X1011X10111010X110110011010101X1110
                mem[42913] = 705556
                mem[51879] = 587
                mem[30832] = 7362
                mem[26448] = 775
                mask = 11101101X110X1111XX11101100001100110
                mem[36838] = 3566
                mem[862] = 27
                mem[18069] = 278016697
                mask = 11111011X0010X10X1X10010001100110111
                mem[44833] = 180646
                mem[11079] = 56305785
                mask = 1110X11101111010X101000XX10X01X0100X
                mem[5155] = 708
                mem[47296] = 115126445
                mem[15398] = 240969
                mask = X0101111XX10X10X10011X01001001X1110X
                mem[31539] = 479329354
                mem[42429] = 103334
                mem[19047] = 17543374
                mem[28669] = 5011
                mem[7597] = 665681
                mask = 1011011010X0110011X00X001000X10XX001
                mem[32285] = 3524273
                mem[41636] = 4242
                mask = 1011XXX1000101X00110000XX0110X1X0010
                mem[41353] = 127618
                mem[25077] = 7895994
                mem[847] = 1382347
                mask = 11011X01011111011011X10111101XX00001
                mem[46056] = 44249
                mem[32502] = 139280364
                mem[61549] = 6979
                mem[63846] = 22024
                mem[12603] = 164483642
                mem[7476] = 818802
                mem[13331] = 5583
                mask = 1111X11XX0111110X1111010110001010001
                mem[50040] = 761
                mem[18607] = 3490944
                mem[29450] = 1752186
                mem[8882] = 1093315
                mem[53747] = 112539
                mask = 110X10010111X1011X110011101X11100010
                mem[42281] = 39707930
                mem[421] = 186443
                mem[23888] = 23798402
                mask = 111011X10X1X011X101XX10X0001011X101X
                mem[42913] = 2858255
                mem[35638] = 694
                mem[4406] = 19405
                mask = 1X101101011001111X011X1010X00XX1100X
                mem[23701] = 348742357
                mem[41142] = 130262
                mem[40526] = 4827465
                mem[18816] = 1018
                mem[38755] = 820376128
                mem[37585] = 961778675
                mask = XX1X1111011X01101101101X1X100XX1X000
                mem[44856] = 514771
                mem[19431] = 4453
                mem[12280] = 4664226
                mem[56303] = 3892960
                mem[380] = 4920448
                mask = 10X1X11XX010X10011001X10000X1101X1X1
                mem[37729] = 1701179
                mem[19336] = 7943601
                mem[18993] = 245
                mem[59647] = 971349
                mem[29349] = 62223
                mem[9752] = 17497525
                mask = 11XX1111X11111011X101001010111101111
                mem[2553] = 469044
                mem[47330] = 20637
                mem[30906] = 348
                mem[57352] = 436423
                mem[63561] = 21743
                mem[59075] = 20664
                mask = 110X11X10X1111X1XX11000X00000100X00X
                mem[11954] = 54118
                mem[22865] = 2837
                mem[43321] = 31
                mask = X01X11X10111X1X0110X1010X00100011X00
                mem[7858] = 11990040
                mem[63568] = 1202476
                mem[10036] = 4966451
                mem[63170] = 27996587
                mem[36887] = 519605252
                mem[46880] = 173
                mask = 1111X1X100X101X0101X01111000000011X0
                mem[7217] = 8228889
                mem[45660] = 314313
                mem[14336] = 874149154
                mask = 11XX11X1011X110XX0X100100100XX110000
                mem[11376] = 653378
                mem[8346] = 577
                mem[60957] = 970
                mem[35419] = 704475
                mem[11948] = 11
                mem[44599] = 3045
                mem[61517] = 6188
                mask = 0X0X01111X1111001111101010X001000110
                mem[15346] = 252687
                mem[9751] = 2501
                mem[19820] = 1102513
                mem[39973] = 50579363
                mem[14097] = 15116201
                mask = 111XX1110011011X1X1X01X1X00X0X001010
                mem[2062] = 1857
                mem[25077] = 112587
                mem[50942] = 95956144
                mem[4414] = 261156
                mask = 010X011X10111X001X11X1X1101X101X1100
                mem[58698] = 1734
                mem[19278] = 1411
                mem[18023] = 418685588
                mem[53936] = 67844200
                mask = X0X0X111X111110011X1101X0101X01X110X
                mem[7258] = 22502
                mem[19431] = 1552153
                mem[59373] = 56076
                mem[48370] = 16144653
                mem[6180] = 10341
                mem[47030] = 53744227
                mask = 1X111111011X11X011010010X1X0XX100X0X
                mem[38329] = 5983
                mem[250] = 114554771
                mem[6015] = 3879253
                mem[26369] = 21038
                mem[54373] = 11996994
                mem[32357] = 83791
                mem[44566] = 248
                mask = 1001X11100100X0X110XX000XX1000011101
                mem[65264] = 66677347
                mem[4781] = 2348
                mem[35217] = 28934660
                mem[62670] = 8204
                mem[4241] = 103518
                mem[60109] = 2030866
                mask = 11011000011110X1X01X00100X0X010X0101
                mem[2921] = 38494
                mem[61032] = 302978
                mem[41205] = 4437528
                mem[54175] = 4086
                mem[53309] = 5872688
                mask = X0111X1100010110X1XX0001000100101100
                mem[2923] = 122
                mem[38270] = 3505
                mem[52130] = 9802092
                mask = X1111X110XX1011011X1X00X0XX101110110
                mem[43637] = 250424
                mem[48047] = 3277
                mem[61526] = 470310
                mem[5741] = 25454525
                mask = 01XX11110101X110111101110000X1010100
                mem[13043] = 53350618
                mem[12662] = 43140071
                mem[43247] = 9723
                mem[4225] = 5492
                mask = 1111111101X11100110X00X00XX010100X00
                mem[24864] = 143000050
                mem[21285] = 13586288
                mem[24001] = 459956
                mem[58137] = 120522965
                mask = 001111XXX111010X11010011000000010000
                mem[65462] = 992
                mem[418] = 519196547
                mem[55624] = 389245
                mem[53286] = 1903013
                mem[29554] = 1498
                mask = 1110111101XX111011010X101X001XX0X001
                mem[57444] = 79509
                mem[49019] = 1058509972
                mask = 11X111110011111X0X01001011X0011X00X1
                mem[59671] = 429281585
                mem[4406] = 5443
                mem[21722] = 2282097
                mem[58297] = 3240
                mem[34945] = 659
                mask = 001X01110111X1101X0110100111000XX10X
                mem[36838] = 498
                mem[30907] = 3256
                mem[7720] = 20794707
                mask = 11X11X100X1111X11X1X0001001101110011
                mem[30681] = 22038101
                mem[10862] = 163635
                mem[7390] = 263
                mem[34048] = 658055
                mask = XX1111111X1111110X0100X001XX1X000100
                mem[50942] = 21752495
                mem[33712] = 2053404
                mem[12280] = 2448814
                mem[60868] = 741
                mask = 11X11X1101XX11001101011001X0001000XX
                mem[51202] = 71310482
                mem[1920] = 1045746
                mem[22700] = 77197
                mask = 1XX11111XX11111XX1X1X01001101111X010
                mem[18070] = 979702
                mem[51517] = 40578850
                mem[10915] = 12864305
                mask = X0X0110101X101XX1101X1001010X0100111
                mem[63388] = 7337420
                mem[50249] = 1611403
                mem[58026] = 11231319
                mem[26752] = 14879054
                mem[55961] = 74697
                mem[13375] = 409
                mem[55211] = 1765
                mask = 0X0X1X11010111101X01101001X010110101
                mem[53404] = 5316907
                mem[9311] = 11078285
                mem[1109] = 100171
                mem[60125] = 3459
                mask = 10X11X1X10101100X100110100001X10X011
                mem[36318] = 4280
                mem[58231] = 2873088
                mem[41636] = 1320902
                mem[34430] = 3498
                mask = 10110110X01X11001100000100X11110110X
                mem[26398] = 340609
                mem[45819] = 794732
                mem[63846] = 74605623
                mem[22088] = 68488
                mem[12276] = 819324
                mask = 1111X11101X0111011010101X1100110000X
                mem[26221] = 403142303
                mem[43628] = 408
                mem[40992] = 836169
                mask = 100101XX1011X1X00101X00X1101110X1100
                mem[4406] = 22245945
                mem[54557] = 252950
                mem[64139] = 636
                mask = 1010X10101111X10110110100XXX0011110X
                mem[24001] = 113366995
                mem[34048] = 248684005
                mem[36838] = 7380278
                mem[47106] = 3768609
                mem[38755] = 3375
                mem[2386] = 4779
                mask = 1X1011X1011X01XX1XX11100101XX0111111
                mem[56303] = 22633
                mem[4128] = 487998
                mem[57652] = 6663511
                mem[46800] = 1277048
                mem[60711] = 625417
                mask = 11011011X1X111011011000101100X1000X0
                mem[44402] = 835733
                mem[32497] = 248789900
                mem[34174] = 1207
                mask = 11101X1X01X01X101101011010000X000100
                mem[29154] = 1468683
                mem[51333] = 5222
                mask = 10010111X0X01100110X001000001X0X01X0
                mem[35096] = 43050
                mem[58911] = 15997316
                mem[17680] = 2223467
                mem[11924] = 1685920
                mem[25199] = 475208411
                mem[26369] = 1006891";
        }
    }
}
