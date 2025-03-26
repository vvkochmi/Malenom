using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace MalenomDesk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Image> images = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e) // Загрузка окна, получение изображений
        {
            try
            {
                images = await new ApiService().GetData();
                ImgGrid.ItemsSource = images;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void AddBut_Click(object sender, RoutedEventArgs e) // Добавление изображения, через выбор в диалоге
        {
            string filePath = "";

            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;"
            };
            if (fileDialog.ShowDialog().Value)
            {
                filePath = fileDialog.FileName;
            }

            Image image = new Image { Name = Path.GetFileName(filePath), Img = File.ReadAllBytes(filePath) };
            try
            {
                await new ApiService().SetData(image);
                images.Add(image);
                ImgGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void ChangeBut_Click(object sender, RoutedEventArgs e) // Изменение изображение, через выбор в диалоге
        {
            Image img = (Image)ImgGrid.SelectedItem;
            if (img == null)
            {
                MessageBox.Show("Запись не выбрана");
                return;
            }

            string filePath = "";

            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;"
            };
            if (fileDialog.ShowDialog().Value)
            {
                filePath = fileDialog.FileName;
            }

            byte[] imgData = File.ReadAllBytes(filePath);
            string imgName = Path.GetFileName(filePath);

            try
            {
                await new ApiService().UpdateImage(img);
                img.Img = imgData;
                img.Name = imgName;
                ImgGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void DelBut_Click(object sender, RoutedEventArgs e) // Удаление изображения
        {
            Image img = (Image)ImgGrid.SelectedItem;
            if (img == null)
            {
                MessageBox.Show("Запись не выбрана");
                return;
            }

            try
            {
                await new ApiService().DeleteData(img.ID);
                images.Remove(img);
                ImgGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
