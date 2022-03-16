using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TMnote
{
    internal static class ImageHelper
    {
        internal static class ICON
        {
            internal static class GLOBAL
            {
                internal static string NORMAL = @"Configuration\\Configuration_Normal-icon.png";
                internal static string WARNING = @"Configuration\\Configuration_Warning-icon.png";
                internal static string CANCEL = @"Configuration\\Configuration_Cancel-icon.png";                                                                                                               // WAS NOT
            }

            internal static class IDIOM
            {
                internal static string ADD = @"Configuration\\Configuration_AddIdiom-icon.png";                                                                                                                // WAS NOT
                internal static string COPY_IDIOM_TO_OTHER_GROUP = @"Configuration\\Configuration_CopyIdiomToOtherGroup-icon.png";                                                       // Had \\ on front
                internal static string COPY_IDIOM_TO_OTHER_GROUP_SELECT_GROUP = @"Configuration\\Configuration_CopyIdiomToOtherGroup_selectGroup-icon.png";                              // Had \\ on front
                internal static string DELETE = @"Configuration\\Configuration_DeleteIdiom-icon.png";                                                                                    // Had \\ on front
                internal static string DUPLICATE = @"Configuration\\Configuration_DuplicateIdiom-icon.png";                                                                              // Had \\ on front
                internal static string MODIFY = @"Configuration\\Configuration_ModifyIdiom-icon.png";                                                                                    // Had \\ on front
                internal static string MOVE_IDIOM_TO_OTHER_GROUP = @"Configuration\\Configuration_MoveIdiomToOtherGroup-icon.png";                                                       // Had \\ on front
                internal static string MOVE_IDIOM_TO_OTHER_GROUP_SELECT_GROUP = @"Configuration\\Configuration_MoveIdiomToOtherGroup_selectGroup-icon.png";      // NOT CALLED - WHY ?   // Had \\ on front
            }

            internal static class ENVIROIDIOM
            {
                internal static string ADD = @"Configuration\\Configuration_AddEnviroIdiom-icon.png";                                                          // NEW
                internal static string DELETE = @"Configuration\\Configuration_DeleteEnviroIdiom-icon.png";                                                    // NEW  // DOUBLED              // Had \\ on front
                internal static string DUPLICATE = @"Configuration\\Configuration_DuplicateEnviroIdiom-icon.png";                                              // NEW  // DOUBLED              // Had \\ on front
                internal static string MODIFY = @"Configuration\\Configuration_ModifyEnviroIdiom-icon.png";                                                    // NEW  // DOUBLED              // Had \\ on front
                internal static string ENVIRO_COPY_IDIOM_TO_OTHER_GROUP = @"Configuration\\Configuration_Enviro_CopyIdiomToOtherGroup-icon.png";
                internal static string ENVIRO_MOVE_IDIOM_TO_OTHER_GROUP = @"Configuration\\Configuration_Enviro_MoveIdiomToOtherGroup-icon.png";
                internal static string COPY_MOVE_IDIOM_TO_OTHER_ENVIROGR = @"Configuration\\Configuration_CopyMoveEnviroIdiomToOtherEnviroGr-icon.png";                                        // Had \\ on front
            }

            internal static class GROUP
            {
                internal static string ADD = @"Configuration\\Configuration_AddGroup-icon.png";                                                                                                                // WAS NOT
                internal static string DELETE = @"Configuration\\Configuration_DeleteGroup-icon.png";
                internal static string MODIFY = @"Configuration\\Configuration_ModifyGroup-icon.png";
                internal static string DUPLICATE = @"Configuration\\Configuration_DuplicateGroup-icon.png";
            }

            internal static class ENVIROGR
            {
                internal static string ADD = @"Configuration\\Configuration_AddEnviroGr-icon.png";                                                                                                             // WAS NOT
                internal static string DELETE = @"Configuration\\Configuration_DeleteEnviroGr-icon.png";
                internal static string MODIFY = @"Configuration\\Configuration_ModifyEnviroGr-icon.png";
                internal static string DUPLICATE = @"Configuration\\Configuration_DuplicateEnviroGr-icon.png";
                internal static string CHANGE_IDIOM_POSITION = @"Configuration\\Configuration_ChangeIdiomPositionEnviroGr-icon.png";
                internal static string COPY_MOVE_IDIOMS_TO_OTHER_ENVIROGR = @"Configuration\\Configuration_CopyMoveIdiomsToOtherEnviroGr-icon.png";                                      // Had \\ on front
                internal static string CREATE_IDIOM_FROM_ENVIROGR_BACKGROUND = @"Configuration\\Configuration_CreateIdiomFromEnviroGrBackground-icon.png";                               // Had \\ on front
                internal static string DELETE_IDIOM = @"Configuration\\Configuration_DeleteEnviroIdiomEnviroGr-icon.png";
            }

            internal static class PATTERN
            {
                internal static string GREEN = @"Muster\\__Muster_Redew_Element_gruen.png";
                internal static string BLUE_DARK = @"Muster\\__Muster_Redew_Gruppe_BlauDunkel.png";
                internal static string BLUE = @"__Muster_Redew_Gruppe_Blau.png";
                internal static string RED = @"Muster\\__Muster_EditModus_rot.png";
                internal static string YELLOW = @"Muster\\__Muster_Konfiguration_Gelb.png";
            }
        }

        public static BitmapImage LoadBitmapImage(string path)
        {
            BitmapImage image = new BitmapImage();

            if (System.IO.File.Exists(path))
            {
                using (var stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(path)))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                }
            }
            return image;
        }

        public static void SaveImageToAFolder(BitmapFrame src)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "PNG Files | *.png";
            dlg.DefaultExt = "png";
            if (dlg.ShowDialog() == true)
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(src));
                using (var stream = dlg.OpenFile())
                {
                    encoder.Save(stream);
                }
            }
        }

        public static void SaveImage(BitmapFrame src, string myFile)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(src));
            using (var file = File.OpenWrite(myFile))
            {
                encoder.Save(file);
            }

        }

        public static BitmapFrame CreateBitmapFrameFromString(string result)
        {
            Uri myUri = new Uri(result);
            return BitmapFrame.Create(myUri);
        }
    }
}

