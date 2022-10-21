using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FactMasiva.Controllers
{
    public class GridViewControl
    {
        public string movimiento;

        public string getMovimiento()
        {
            return this.movimiento;
        }

        public void setMovimiento(string mov)
        {
            this.movimiento = mov;
        }

        public BoundField CreateBoundColumn(
          string DataFieldValue,
          string HeaderTextValue,
          int ancho)
        {
            BoundField boundField = new BoundField();
            boundField.DataField = DataFieldValue;
            boundField.HeaderText = HeaderTextValue;
            boundField.ItemStyle.Width = Unit.Point(ancho);
            boundField.ItemStyle.Height = Unit.Point(10);
            boundField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            return boundField;
        }

        public HyperLinkField createHyperLink(
          string DataFieldValue,
          string HeaderTextValue,
          int ancho)
        {
            HyperLinkField hyperLinkField = new HyperLinkField();
            hyperLinkField.DataNavigateUrlFields = new string[1]
            {
        DataFieldValue
            };
            hyperLinkField.HeaderText = HeaderTextValue;
            hyperLinkField.ItemStyle.Width = Unit.Point(ancho);
            hyperLinkField.ItemStyle.Height = Unit.Point(10);
            hyperLinkField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            hyperLinkField.Text = HeaderTextValue;
            hyperLinkField.Target = "_blank";
            return hyperLinkField;
        }

        public TemplateField prueba()
        {
            TemplateField templateField = new TemplateField();
            templateField.HeaderText = "Disposition";
            templateField.ItemTemplate = (ITemplate)new GridViewControl.DropDownListControl();
            return templateField;
        }

        public TemplateField etiquetaRendimiento()
        {
            TemplateField templateField = new TemplateField();
            templateField.HeaderText = "Rendimiento";
            templateField.ItemTemplate = (ITemplate)new GridViewControl.Etiqueta(this.getMovimiento());
            return templateField;
        }

        public TemplateField check()
        {
            TemplateField templateField = new TemplateField();
            templateField.HeaderText = "";
            templateField.ItemTemplate = (ITemplate)new GridViewControl.CheckBoxControl();
            templateField.ItemStyle.Width = new Unit(5.0, UnitType.Percentage);
            templateField.ItemStyle.VerticalAlign = VerticalAlign.Middle;
            return templateField;
        }

        private class Etiqueta : ITemplate
        {
            public string mov;

            public void setMov(string movi)
            {
                this.mov = movi;
            }

            public Etiqueta(string movi)
            {
                this.mov = movi;
            }

            public void InstantiateIn(Control container)
            {
                Label label = new Label();
                label.ID = "Rendimiento";
                double num = 1.0;
                label.Text = num.ToString();
                label.Font.Bold = true;
                container.Controls.Add((Control)label);
            }
        }

        private class DropDownListControl : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                DropDownList dropDownList = new DropDownList();
                dropDownList.ID = "accion";
                dropDownList.Items.Add(new ListItem("Reciclar", "Y"));
                dropDownList.Items.Add(new ListItem("Destruir", "D"));
                container.Controls.Add((Control)dropDownList);
            }
        }

        private class CheckBoxControl : ITemplate
        {
            public void InstantiateIn(Control conta)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.ID = "check";
                conta.Controls.Add((Control)checkBox);
            }
        }
    }
}