﻿using PakReader;
using PakReader.Pak;
using PakReader.Parsers.Objects;
using SkiaSharp;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
#nullable enable

namespace MCDSaveEdit
{

    public static class ImageUriHelper
    {
        public static IImageResolver instance = new LocalImageResolver();

        public static string? usableGameContentIfExists()
        {
            string? winstorePath = Constants.WINSTORE_PAKS_FOLDER_IF_EXISTS;
            if (!string.IsNullOrWhiteSpace(winstorePath) && Directory.Exists(winstorePath))
            {
                return winstorePath;
            }
            string launcherPath = Constants.PAKS_FOLDER_PATH;
            if (Directory.Exists(launcherPath))
            {
                return launcherPath;
            }
            return null;
        }

        public static bool gameContentLoaded { get; private set; } = false;
        public static async Task loadGameContentAsync(string paksFolderPath)
        {
            var pakIndex = await loadPakIndex(paksFolderPath);
            if (pakIndex != null)
            {
                var pakImageResolver = new PakImageResolver(pakIndex, paksFolderPath);
                await pakImageResolver.loadPakFilesAsync(preloadBitmaps: false);
                instance = pakImageResolver;
                gameContentLoaded = true;
            }
        }

        private static Task<PakIndex?> loadPakIndex(string paksFolderPath)
        {
            var tcs = new TaskCompletionSource<PakIndex?>();
            Task.Run(() =>
            {
                try
                {
                    var filter = new PakFilter(new[] { Constants.PAKS_FILTER_STRING }, false);
                    var pakIndex = new PakIndex(path: paksFolderPath, cacheFiles: true, caseSensitive: true, filter: filter);
                    if (!unlockPakIndex(pakIndex))
                    {
                        throw new ArgumentException($"Could not decrypt pak files at {paksFolderPath}", "paksFolderPath");
                    }
                    tcs.SetResult(pakIndex);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not load Minecraft Dungeons Paks: {e}");
                    tcs.SetException(e);
                }
            });
            return tcs.Task;
        }

        private static bool unlockPakIndex(PakIndex pakIndex)
        {
            foreach(var keyStr in Secrets.PAKS_AES_KEY_STRINGS)
            {
                byte[] keyBytes;
                if (keyStr.StartsWith("0x"))
                {
                    keyBytes = keyStr.Substring(2).ToBytesKey();
                }
                else
                {
                    keyBytes = keyStr.ToBytesKey();
                }
                var count = pakIndex.UseKey(FGuid.Zero, keyBytes);
                if(count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static BitmapImage bitmapImageFromSKBitmap(SKBitmap image) => bitmapImageFromSKImage(SKImage.FromBitmap(image));
        public static BitmapImage bitmapImageFromSKImage(SKImage image)
        {
            using var encoded = image.Encode();
            using var stream = encoded.AsStream();
            BitmapImage photo = new BitmapImage();
            photo.BeginInit();
            photo.CacheOption = BitmapCacheOption.OnLoad;
            photo.StreamSource = stream;
            photo.EndInit();
            photo.Freeze();
            return photo;
        }


        /// <summary>
        /// Load a resource WPF-BitmapImage (png, bmp, ...) from embedded resource defined as 'Resource' not as 'Embedded resource'.
        /// </summary>
        /// <param name="pathInApplication">Path without starting slash</param>
        private static BitmapImage loadBitmapFromResource(string pathInApplication)
        {
            if (pathInApplication[0] == '/')
            {
                pathInApplication = pathInApplication.Substring(1);
            }
            var rootPath = @"pack://application:,,/";
            var fullPath = Path.Combine(rootPath, pathInApplication);
            var uri = new Uri(fullPath, UriKind.RelativeOrAbsolute);
            Console.WriteLine(uri);
            return new BitmapImage(uri);
        }

        private static string spaceOutWords(string input)
        {
            var output = new StringBuilder();
            for (int ii = 0; ii < input.Length; ii++)
            {
                var letter = input[ii];
                if (ii > 0 && char.IsUpper(letter) && output[output.Length] != ' ')
                {
                    output.Append(' ');
                }
                output.Append(letter);
            }
            return output.ToString();
        }
    }
}
