using System.Globalization;

namespace Practica4_Calculadora_Daferlin;

public partial class MainPage : ContentPage
{
    private double primerNumero = 0;
    private double segundoNumero = 0;
    private string operadorActual = "";
    private bool operacionPendiente = false;

    public MainPage()
    {
        InitializeComponent();
    }

    private static string Fmt(double n) =>
        n.ToString("G15", CultureInfo.InvariantCulture);

    // Muestra la operación en curso: "5 +" o "5 + 3"
    private void ActualizarExpresion()
    {
        if (operadorActual == "") return;

        string segundo = operacionPendiente ? "" : lblDisplay.Text;
        lblExpresion.Text = $"{Fmt(primerNumero)} {operadorActual} {segundo}".TrimEnd();
    }

    private void OnNumberClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button) return;
        string valor = button.Text;

        // Tras un operador, un resultado o un "Error", se empieza de cero
        if (lblDisplay.Text == "Error" || operacionPendiente)
        {
            // Si no hay operador, es una operación nueva: se limpia la expresión
            if (operadorActual == "") lblExpresion.Text = "";

            lblDisplay.Text = valor == "." ? "0." : valor;
            operacionPendiente = false;
        }
        else if (valor == ".")
        {
            // Solo se permite un punto decimal
            if (!lblDisplay.Text.Contains('.'))
                lblDisplay.Text += ".";
        }
        else
        {
            // Evita "00", "05", etc.
            lblDisplay.Text = lblDisplay.Text == "0" ? valor : lblDisplay.Text + valor;
        }

        ActualizarExpresion();
    }

    private void OnOperatorClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button) return;
        if (lblDisplay.Text == "Error") return;

        // Guardar el primer número y el operador
        primerNumero = double.Parse(lblDisplay.Text, CultureInfo.InvariantCulture);
        operadorActual = button.Text;

        // El siguiente número que se escriba reemplaza al display
        operacionPendiente = true;

        ActualizarExpresion();
    }

    private void OnEqualsClicked(object? sender, EventArgs e)
    {
        // Sin operador pendiente (o con "Error") no hay nada que calcular
        if (operadorActual == "" || lblDisplay.Text == "Error") return;

        segundoNumero = double.Parse(lblDisplay.Text, CultureInfo.InvariantCulture);
        lblExpresion.Text = $"{Fmt(primerNumero)} {operadorActual} {Fmt(segundoNumero)} =";

        double resultado = 0;

        switch (operadorActual)
        {
            case "+":
                resultado = primerNumero + segundoNumero;
                break;
            case "-":
                resultado = primerNumero - segundoNumero;
                break;
            case "×":
                resultado = primerNumero * segundoNumero;
                break;
            case "÷":
                if (segundoNumero == 0)
                {
                    lblDisplay.Text = "Error";
                    operadorActual = "";
                    operacionPendiente = false;
                    return;
                }
                resultado = primerNumero / segundoNumero;
                break;
        }

        lblDisplay.Text = Fmt(resultado);

        operadorActual = "";
        operacionPendiente = true;
    }

    private void OnClearClicked(object? sender, EventArgs e)
    {
        lblDisplay.Text = "0";
        lblExpresion.Text = "";
        primerNumero = 0;
        segundoNumero = 0;
        operadorActual = "";
        operacionPendiente = false;
    }
}