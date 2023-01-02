using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mazer.Helpers
{
    public static class SaveImageHelper
    {
       
        public static void SavePngImage(System.Windows.Controls.Canvas canvas, string folderType)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string applicationFolder = "Mazer Save Files";
            //Gets the path for the myDocuments path for whatever computer this program is on.
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //combines these paths
            string applicationPath = System.IO.Path.Combine(myDocuments, applicationFolder, string.Format("{0} Mazer Images", folderType));
            //Sees if this path already exists
            if (!Directory.Exists(applicationPath))
            {
                Directory.CreateDirectory(applicationPath);
            }
            //All this provides the usual "Save As" function of any common microsoft application.
            saveFileDialog.Filter = "png (*.png)|*.png|All Files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.Title = "Save Mazer File";
            saveFileDialog.InitialDirectory = applicationPath;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.DefaultExt = "gol";

            //dialogResult can be true, false or null (just in case)
            bool? dialogResult = saveFileDialog.ShowDialog();

            //If what the user enters is Save (OK) will save the file.
            if (dialogResult == true)
            {
                string saveFilePath = saveFileDialog.FileName;

                // Save current canvas transform
                Transform transform = canvas.LayoutTransform;
                // reset current transform (in case it is scaled or rotated)
                canvas.LayoutTransform = null;

                // Get the size of canvas
                Size size = new Size(canvas.ActualWidth, canvas.ActualHeight);
                // Measure and arrange the canvas
                // VERY IMPORTANT
                canvas.Measure(size);
                canvas.Arrange(new Rect(size));

                // Create a render bitmap and push the canvas to it
                RenderTargetBitmap renderBitmap =
                  new RenderTargetBitmap(
                    (int)size.Width,
                    (int)size.Height,
                    96d,
                    96d,
                    PixelFormats.Pbgra32);
                renderBitmap.Render(canvas);

                string currentPath = System.IO.Path.Combine(myDocuments, applicationFolder, string.Format("{0} Mazer Images", folderType), saveFilePath);

                // Create a file stream for saving image
                using (FileStream outStream = new FileStream(currentPath, FileMode.Create))
                {
                    // Use png encoder for our data
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    // push the rendered bitmap to it
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    // save the data to the stream
                    encoder.Save(outStream);
                }

                // Restore previously saved layout
                canvas.LayoutTransform = transform;
            }
        }
    }
}
