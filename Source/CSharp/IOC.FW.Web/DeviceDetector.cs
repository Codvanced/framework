using System;
using System.Text.RegularExpressions;

//TODO: Estudar a implementação de um Dictionary para fazer a comparação
namespace IOC.FW.Web
{
    /// <summary>
    /// Classe para identificar o tipo de dispositivo 
    /// baseado no Categorizr https://github.com/bjankord/Categorizr
    /// </summary>
    public class DeviceDetector
    {
        /// <summary>
        /// Tipos de dispositivos
        /// </summary>
        public enum DeviceType
        {
            DESKTOP,
            TV,
            TABLET,
            MOBILE
        }

        /// <summary>
        /// Identifica o dispositivo de acordo com o user agent
        /// </summary>
        /// <param name="userAgent">O user agent para ser analisado</param>
        /// <param name="IsTabletsDesktops">Se true, considera que tablets são desktops</param>
        /// <param name="IsTVsDesktops">Se true, considera que tvs são desktops</param>
        /// <returns>Tipo do dispositivo verificado</returns>
        public static DeviceType GetDeviceType(string userAgent, bool IsTabletsDesktops = false, bool IsTVsDesktops = false)
        {

            DeviceType type = DeviceType.DESKTOP;

            if (!String.IsNullOrEmpty(userAgent))
            {

                #region Regexes

                /* TV */
                Regex smartvRegex =
                    new Regex(@"GoogleTV|SmartTV|Internet.TV|NetCast|NETTV|AppleTV|boxee|Kylo|Roku|DLNADOC|CE\-HTML",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex tvBasedGaminConsole =
                    new Regex(@"Xbox|PLAYSTATION.3|Wii",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                /* TABLET */
                Regex tablet =
                    new Regex(@"iP(a|ro)d",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex tablet2 =
                    new Regex(@"tablet",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex tablet3 =
                    new Regex(@"RX-34",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex tablet4 =
                    new Regex(@"FOLIO",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                /* Android Tablet */
                Regex androidTablet =
                    new Regex(@"Linux",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex androidTablet2 =
                    new Regex(@"Android",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex androidTablet3 =
                    new Regex(@"Fennec|mobi|HTC.Magic|HTCX06HT|Nexus.One|SC-02B|fone.945",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                /* Kindle or Kindle Fire */
                Regex kindle =
                    new Regex(@"Kindle",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex kindle2 =
                    new Regex(@"Mac.OS",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex kindle3 =
                    new Regex(@"Silk",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                /* pre Android 3.0 Tablet */
                Regex android30tablet =
                    new Regex(@"GT-P10|SC-01C|SHW-M180S|SGH-T849|SCH-I800|SHW-M180L|SPH-P100|SGH-I987|zt180|HTC(.Flyer|_Flyer)|Sprint.ATP51|ViewPad7|pandigital(sprnova|nova)|Ideos.S7|Dell.Streak.7|Advent.Vega|A101IT|A70BHT|MID7015|Next2|nook",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex android30tablet2 =
                    new Regex(@"MB511",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex android30tablet3 =
                    new Regex(@"RUTEM",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                /* Opera User Agent */

                Regex opera =
                    new Regex(@"BOLT|Fennec|Iris|Maemo|Minimo|Mobi|mowser|NetFront|Novarra|Prism|RX-34|Skyfire|Tear|XV6875|XV6975|Google.Wireless.Transcoder",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex opera2 =
                    new Regex(@"Opera",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex opera3 =
                    new Regex(@"Windows.NT.5",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex opera4 =
                    new Regex(@"HTC|Xda|Mini|Vario|SAMSUNG\-GT\-i8000|SAMSUNG\-SGH\-i9",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                /* Windows Desktop */

                Regex winDesktop =
                    new Regex(@"Windows.(NT|XP|ME|9)",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex winDesktop2 =
                    new Regex(@"Phone",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex winDesktop3 =
                    new Regex(@"Win(9|.9|NT)",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                /* Mac Desktop */

                Regex macDesktop =
                    new Regex(@"Macintosh|PowerPC",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex macDesktop2 =
                    new Regex(@"Silk",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                /* Linux Desktop */

                Regex linuxDesktop =
                    new Regex(@"Linux",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex linuxDesktop2 =
                    new Regex(@"X11",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                /* Solaris, SunOS, BSD Desktop */

                Regex solaris =
                    new Regex(@"Solaris|SunOS|BSD",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                /* Desktop BOT/Crawler/Spider */

                Regex desktopBot =
                    new Regex(@"Bot|Crawler|Spider|Yahoo|ia_archiver|Covario-IDS|findlinks|DataparkSearch|larbin|Mediapartners-Google|NG-Search|Snappy|Teoma|Jeeves|TinEye",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);

                Regex desktopBot2 =
                    new Regex(@"Mobile",
                        RegexOptions.Compiled | RegexOptions.IgnoreCase);


                #endregion

                // Check if user agent is a smart TV - http://goo.gl/FocDk
                if (smartvRegex.IsMatch(userAgent))
                    type = DeviceType.TV;

                // Check if user agent is a TV Based Gaming Console
                else if (tvBasedGaminConsole.IsMatch(userAgent))
                    type = DeviceType.TV;

                // Check if user agent is a Tablet
                else if (tablet.IsMatch(userAgent)
                        || tablet2.IsMatch(userAgent)
                        && !tablet3.IsMatch(userAgent) || tablet4.IsMatch(userAgent))
                    type = DeviceType.TABLET;

                // Check if user agent is an Android Tablet
                else if (androidTablet.IsMatch(userAgent)
                        && androidTablet2.IsMatch(userAgent)
                        && !androidTablet3.IsMatch(userAgent))
                    type = DeviceType.TABLET;

                // Check if user agent is a Kindle or Kindle Fire
                else if (kindle.IsMatch(userAgent)
                        || kindle2.IsMatch(userAgent)
                        && kindle3.IsMatch(userAgent))
                    type = DeviceType.TABLET;

                // Check if user agent is a pre Android 3.0 Tablet
                else if (android30tablet.IsMatch(userAgent)
                        || android30tablet2.IsMatch(userAgent)
                        && android30tablet3.IsMatch(userAgent))
                    type = DeviceType.TABLET;

                // Check if user agent is unique Mobile User Agent
                else if (opera.IsMatch(userAgent))
                    type = DeviceType.MOBILE;

                // Check if user agent is an odd Opera User Agent - http://goo.gl/nK90K
                else if (opera2.IsMatch(userAgent)
                        && opera3.IsMatch(userAgent)
                        && opera4.IsMatch(userAgent))
                    type = DeviceType.MOBILE;

                // Check if user agent is Windows Desktop
                else if (winDesktop.IsMatch(userAgent)
                        && !winDesktop2.IsMatch(userAgent)
                        || winDesktop3.IsMatch(userAgent))
                    type = DeviceType.DESKTOP;

                // Check if user agent is Mac Desktop
                else if (macDesktop.IsMatch(userAgent)
                        && !macDesktop2.IsMatch(userAgent))
                    type = DeviceType.DESKTOP;

                // Check if user agent is a Linux Desktop
                else if (linuxDesktop.IsMatch(userAgent)
                        && linuxDesktop2.IsMatch(userAgent))
                    type = DeviceType.DESKTOP;

                // Check if user agent is a Solaris, SunOS, BSD Desktop
                else if (solaris.IsMatch(userAgent))
                    type = DeviceType.DESKTOP;

                // Check if user agent is a Desktop BOT/Crawler/Spider
                else if (desktopBot.IsMatch(userAgent)
                        && !desktopBot2.IsMatch(userAgent))
                    type = DeviceType.DESKTOP;

                // Otherwise assume it is a Mobile Device
                else
                    type = DeviceType.MOBILE;
            }

            if (IsTabletsDesktops && type == DeviceType.TABLET)
                type = DeviceType.DESKTOP;

            if (IsTVsDesktops && type == DeviceType.TV)
                type = DeviceType.DESKTOP;

            return type;
        }
    }
}