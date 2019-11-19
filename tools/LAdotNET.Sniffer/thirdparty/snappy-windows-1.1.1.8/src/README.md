Quoting from [upstream Snappy](http://code.google.com/p/snappy/) homepage:

> Snappy is a compression/decompression library. It does not aim for maximum compression, or compatibility with any other compression library;
> instead, it aims for very high speeds and reasonable compression. For instance, compared to the fastest mode of zlib,
> Snappy is an order of magnitude faster for most inputs, but the resulting compressed files are anywhere from 20% to 100% bigger.
> On a single core of a Core i7 processor in 64-bit mode, Snappy compresses at about 250 MB/sec or more and decompresses at about 500 MB/sec or more.

This is a Windows port of Snappy. It builds out of the box in Visual C++ 2013.
It produces nice clean DLLs with both 32-bit and 64-bit versions available.

There is also an [older Windows port of Snappy](https://snappy4net.codeplex.com/) by author of LZ4.
This port incorporates everything in the older port, but it uses newer version of Snappy,
links statically to C++ runtime, exposes both C and C++ APIs through DLL symbol exports,
provides more complete download, and generally better experience for C++ developers.
.NET wrapper is provided through separate project [Snappy.NET](https://bitbucket.org/robertvazan/snappy.net).

You can download the DLLs, LIB, and header files here:

[snappy-dlls-1.1.1-rev3.zip](https://bitbucket.org/robertvazan/snappy-visual-cpp/downloads/snappy-dlls-1.1.1-rev3.zip)

## Tests and benchmarks

Tests have been ported as well and they show that this Windows port is correct and fast.
The various benchmark types should be interpreted as follows:

* BM_ZFlat - compression speed and compression ratio (compressed size / uncompressed size)
* BM_UFlat - decompression speed
* BM_UValidate - validation of compressed stream

Speed benchmarks should be taken with a grain of salt.
The CPU used for testing was a very fast Core i7 3.4GHz with all data fitting in its L3 cache.
Benchmarks have been done on a single core with all the other cores unoccupied.

## 32-bit test results

	C:\...\snappy-visual-cpp>Release\runtests.exe
	Running microbenchmarks.
	Benchmark            Time(ns)    CPU(ns) Iterations
	---------------------------------------------------
	BM_UFlat/0              86114      90398       1208 1.1GB/s  html
	BM_UFlat/1             841390     852105        238 785.8MB/s  urls
	BM_UFlat/2               5516       5662      33057 20.9GB/s  jpg
	BM_UFlat/3                131        128    1333333 1.4GB/s  jpg_200
	BM_UFlat/4              28449      28278       6620 3.1GB/s  pdf
	BM_UFlat/5             345159     347857        583 1.1GB/s  html4
	BM_UFlat/6              30515      30464       6657 770.2MB/s  cp
	BM_UFlat/7              13173      13010      15588 817.3MB/s  c
	BM_UFlat/8               3805       3782      53619 938.2MB/s  lsp
	BM_UFlat/9            1249163    1275477        159 769.9MB/s  xls
	BM_UFlat/10               303        308     606060 617.5MB/s  xls_200
	BM_UFlat/11            276564     262186        714 553.2MB/s  txt1
	BM_UFlat/12            243323     241429        840 494.5MB/s  txt2
	BM_UFlat/13            740712     690778        271 589.2MB/s  txt3
	BM_UFlat/14           1011155    1050782        193 437.3MB/s  txt4
	BM_UFlat/15            402018     408050        497 1.2GB/s  bin
	BM_UFlat/16               266        273     512820 696.7MB/s  bin_200
	BM_UFlat/17             50228      51057       3972 714.3MB/s  sum
	BM_UFlat/18              4866       4829      38759 834.6MB/s  man
	BM_UFlat/19             81618      81972       2474 1.3GB/s  pb
	BM_UFlat/20            304378     305423        664 575.5MB/s  gaviota
	BM_UValidate/0          39102      39478       5137 2.4GB/s  html
	BM_UValidate/1         435403     406075        461 1.6GB/s  urls
	BM_UValidate/2            190        196     952380 601.5GB/s  jpg
	BM_UValidate/3             73         70    2222222 2.7GB/s  jpg_200
	BM_UValidate/4          13005      12112      15455 7.3GB/s  pdf
	BM_ZFlat/0             194230     193143       1050 505.6MB/s  html (22.31 %)
	BM_ZFlat/1            2245170    2184010        100 306.6MB/s  urls (47.77 %)
	BM_ZFlat/2              36788      29345       5316 4.0GB/s  jpg (99.87 %)
	BM_ZFlat/3                554        568     246913 335.4MB/s  jpg_200 (79.00 %)
	BM_ZFlat/4              95347      87970       2128 1022.6MB/s  pdf (82.07 %)
	BM_ZFlat/5             748713     745591        272 523.9MB/s  html4 (22.51 %)
	BM_ZFlat/6              83891      80759       2318 290.5MB/s  cp (48.12 %)
	BM_ZFlat/7              31498      32165       4365 330.6MB/s  c (42.40 %)
	BM_ZFlat/8               9403       9542      21253 371.9MB/s  lsp (48.37 %)
	BM_ZFlat/9            2274370    2340020        100 419.7MB/s  xls (41.23 %)
	BM_ZFlat/10               705        694     224719 274.8MB/s  xls_200 (78.00 %)
	BM_ZFlat/11            681761     680540        298 213.1MB/s  txt1 (57.87 %)
	BM_ZFlat/12            602058     598233        339 199.6MB/s  txt2 (61.93 %)
	BM_ZFlat/13           1859000    1877787        108 216.7MB/s  txt3 (54.92 %)
	BM_ZFlat/14           2377800    2340020        100 196.4MB/s  txt4 (66.22 %)
	BM_ZFlat/15            663420     676003        300 724.0MB/s  bin (18.11 %)
	BM_ZFlat/16               203        197     869565 966.5MB/s  bin_200 (7.50 %)
	BM_ZFlat/17            139682     132860       1409 274.5MB/s  sum (48.96 %)
	BM_ZFlat/18             12794      12318      15197 327.3MB/s  man (59.36 %)
	BM_ZFlat/19            159285     160316       1265 705.4MB/s  pb (19.64 %)
	BM_ZFlat/20            555553     558680        363 314.6MB/s  gaviota (37.72 %)
	
	
	Running correctness tests.
	All tests passed.

## 64-bit test results

	C:\...\snappy-visual-cpp>x64\Release\runtests.exe
	Running microbenchmarks.
	Benchmark            Time(ns)    CPU(ns) Iterations
	---------------------------------------------------
	BM_UFlat/0              59839      59391       1576 1.6GB/s  html
	BM_UFlat/1             616407     625929        324 1.0GB/s  urls
	BM_UFlat/2               7089       7301      23501 16.2GB/s  jpg
	BM_UFlat/3                 83         77    1818181 2.4GB/s  jpg_200
	BM_UFlat/4              19813      19610       9546 4.5GB/s  pdf
	BM_UFlat/5             249815     253501        800 1.5GB/s  html4
	BM_UFlat/6              19922      19143       9779 1.2GB/s  cp
	BM_UFlat/7               9434       9266      20202 1.1GB/s  c
	BM_UFlat/8               2584       2499      74906 1.4GB/s  lsp
	BM_UFlat/9             936381     943260        215 1.0GB/s  xls
	BM_UFlat/10               215        206     377358 922.7MB/s  xls_200
	BM_UFlat/11            205981     204849        990 708.0MB/s  txt1
	BM_UFlat/12            183019     188127       1078 634.6MB/s  txt2
	BM_UFlat/13            542320     530314        353 767.4MB/s  txt3
	BM_UFlat/14            757496     709094        264 648.1MB/s  txt4
	BM_UFlat/15            306537     304506        666 1.6GB/s  bin
	BM_UFlat/16               209        215     869565 886.0MB/s  bin_200
	BM_UFlat/17             35983      35401       5288 1.0GB/s  sum
	BM_UFlat/18              3530       3437      58997 1.1GB/s  man
	BM_UFlat/19             57265      59177       3427 1.9GB/s  pb
	BM_UFlat/20            216308     204145        917 861.1MB/s  gaviota
	BM_UValidate/0          35711      36149       5610 2.6GB/s  html
	BM_UValidate/1         415625     387579        483 1.7GB/s  urls
	BM_UValidate/2            161        154     606060 765.6GB/s  jpg
	BM_UValidate/3             52         51    3333333 3.6GB/s  jpg_200
	BM_UValidate/4          12067      12249      16556 7.2GB/s  pdf
	BM_ZFlat/0             127627     129502       1566 754.1MB/s  html (22.31 %)
	BM_ZFlat/1            1647181    1676041        121 399.5MB/s  urls (47.77 %)
	BM_ZFlat/2              33583      23252       6038 5.1GB/s  jpg (99.87 %)
	BM_ZFlat/3                464        463     303030 411.7MB/s  jpg_200 (79.00 %)
	BM_ZFlat/4              53554      55213       3673 1.6GB/s  pdf (82.07 %)
	BM_ZFlat/5             527481     525391        386 743.5MB/s  html4 (22.51 %)
	BM_ZFlat/6              53204      52971       3534 442.9MB/s  cp (48.12 %)
	BM_ZFlat/7              19206      19123       9789 556.0MB/s  c (42.40 %)
	BM_ZFlat/8               6457       5389      28943 658.4MB/s  lsp (48.37 %)
	BM_ZFlat/9            1650286    1662303        122 590.8MB/s  xls (41.23 %)
	BM_ZFlat/10               589        574     298507 331.8MB/s  xls_200 (78.00 %)
	BM_ZFlat/11            494736     499509        406 290.4MB/s  txt1 (57.87 %)
	BM_ZFlat/12            450292     452680        448 263.7MB/s  txt2 (61.93 %)
	BM_ZFlat/13           1332614    1370277        148 297.0MB/s  txt3 (54.92 %)
	BM_ZFlat/14           1838211    1860559        109 247.0MB/s  txt4 (66.22 %)
	BM_ZFlat/15            504965     468002        400 1.0GB/s  bin (18.11 %)
	BM_ZFlat/16               203        196     952380 970.4MB/s  bin_200 (7.50 %)
	BM_ZFlat/17             97675      88636       1936 411.4MB/s  sum (48.96 %)
	BM_ZFlat/18              8915       8723      21459 462.1MB/s  man (59.36 %)
	BM_ZFlat/19            115207     119224       1701 948.6MB/s  pb (19.64 %)
	BM_ZFlat/20            410089     404792        501 434.3MB/s  gaviota (37.72 %)
	
	
	Running correctness tests.
	Crazy decompression lengths not checked on 64-bit build
	All tests passed.
