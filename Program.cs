﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

public class Program
{
    public static void Main()
    {
        Console.Title = "RvcWavAudioCompressor | Made by https://github.com/ZygoteCode/";

        if (!Directory.Exists("inputs"))
        {
            Directory.CreateDirectory("inputs");
        }

        if (!Directory.Exists("outputs"))
        {
            Directory.CreateDirectory("outputs");
        }
        else
        {
            Directory.Delete("outputs", true);
            Directory.CreateDirectory("outputs");
        }

        foreach (string file in Directory.GetFiles("inputs"))
        {
            if (Path.GetExtension(file).ToLower().Equals(".wav"))
            {
                new Thread(() => RunFFMpeg($"-i \"{Path.GetFullPath(file)}\" -af aresample=osf=s16:dither_method=triangular_hp -sample_fmt s16 -ar 48000 -ac 1 -b:a 96k -acodec pcm_s16le -filter:a \"highpass=f=50, lowpass=f=15000\" -map a \"{Path.GetFullPath("outputs")}\\{Path.GetFileName(file)}\"")).Start(); ;
            }
        }
    }

    private static void RunFFMpeg(string arguments)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "ffmpeg.exe",
            Arguments = $"-threads {Environment.ProcessorCount} {arguments}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        }).WaitForExit();
    }
}
