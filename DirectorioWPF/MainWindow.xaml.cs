using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using DirectorioWPF.Models;

namespace DirectorioWPF
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient;
        private PersonaViewModel _personaViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7055/api/") };
            _personaViewModel = new PersonaViewModel();
            DataContext = _personaViewModel;
            LoadPersonas();
            LoadPersonasForComboBox();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                var response = await _httpClient.PostAsJsonAsync("personas", _personaViewModel);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Persona registrada exitosamente");
                    ClearFields();
                    LoadPersonas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                if (MessageBox.Show("¿Está seguro de eliminar esta persona?", "Confirmar",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var response = await _httpClient.DeleteAsync($"personas/{id}");
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Persona eliminada exitosamente");
                            LoadPersonas();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
        }

        private bool ValidateFields()
        {
            bool isValid = true;

            txtErrorNombre.Visibility = string.IsNullOrEmpty(_personaViewModel.Nombre) ?
                Visibility.Visible : Visibility.Collapsed;
            txtErrorApellidoPaterno.Visibility = string.IsNullOrEmpty(_personaViewModel.ApellidoPaterno) ?
                Visibility.Visible : Visibility.Collapsed;
            txtErrorIdentificacion.Visibility = string.IsNullOrEmpty(_personaViewModel.Identificacion) ?
                Visibility.Visible : Visibility.Collapsed;

            if (string.IsNullOrEmpty(_personaViewModel.Nombre) ||
                string.IsNullOrEmpty(_personaViewModel.ApellidoPaterno) ||
                string.IsNullOrEmpty(_personaViewModel.Identificacion))
            {
                isValid = false;
            }

            return isValid;
        }

        private async void LoadPersonas()
        {
            try
            {
                var personas = await _httpClient.GetFromJsonAsync<List<PersonaViewModel>>("personas");
                dgPersonas.ItemsSource = personas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar personas: {ex.Message}");
            }
        }

        private void ClearFields()
        {
            _personaViewModel = new PersonaViewModel();
            DataContext = _personaViewModel;
        }

        private async void LoadPersonasForComboBox()
        {
            try
            {
                var personas = await _httpClient.GetFromJsonAsync<List<PersonaViewModel>>("personas");
                cmbPersonas.ItemsSource = personas.Select(p => new
                {
                    Id = p.Id,
                    NombreCompleto = $"{p.Nombre} {p.ApellidoPaterno} {p.ApellidoMaterno}".Trim()
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar personas: {ex.Message}");
            }
        }

        private async void BtnRegistrarVenta_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPersonas.SelectedValue == null ||
                string.IsNullOrEmpty(txtNumeroFactura.Text) ||
                string.IsNullOrEmpty(txtMonto.Text) ||
                dpFecha.SelectedDate == null)
            {
                MessageBox.Show("Por favor complete todos los campos obligatorios");
                return;
            }

            try
            {
                var factura = new
                {
                    numeroFactura = txtNumeroFactura.Text,
                    monto = decimal.Parse(txtMonto.Text),
                    fecha = dpFecha.SelectedDate.Value,
                    personaId = (int)cmbPersonas.SelectedValue,
                    descripcion = txtDescripcion.Text,
                    nombrePersona= cmbPersonas.SelectedItem.ToString(),
                    identificacionPersona=""
                };

                var response = await _httpClient.PostAsJsonAsync("ventas", factura);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Venta registrada exitosamente");
                    ClearVentaFields();
                    LoadVentas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async void LoadVentas()
        {
            try
            {
                var ventas = await _httpClient.GetFromJsonAsync<List<FacturaViewModel>>("ventas");
                dgVentas.ItemsSource = ventas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ventas: {ex.Message}");
            }
        }

        private async void BtnEliminarVenta_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                if (MessageBox.Show("¿Está seguro de eliminar esta venta?", "Confirmar",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var response = await _httpClient.DeleteAsync($"ventas/{id}");
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Venta eliminada exitosamente");
                            LoadVentas();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
            }
        }

        private void ClearVentaFields()
        {
            cmbPersonas.SelectedIndex = -1;
            txtNumeroFactura.Clear();
            txtMonto.Clear();
            dpFecha.SelectedDate = null;
            txtDescripcion.Clear();
        }
    }
}