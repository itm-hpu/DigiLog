namespace LabManager
{
    partial class FilterBefore
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pareto = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.distribution = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.pareto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.distribution)).BeginInit();
            this.SuspendLayout();
            // 
            // pareto
            // 
            chartArea1.Name = "ChartArea1";
            this.pareto.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Frequency";
            legend2.Enabled = false;
            legend2.Name = "Percent";
            this.pareto.Legends.Add(legend1);
            this.pareto.Legends.Add(legend2);
            this.pareto.Location = new System.Drawing.Point(21, 22);
            this.pareto.Name = "pareto";
            series1.ChartArea = "ChartArea1";
            series1.Color = System.Drawing.Color.Navy;
            series1.IsValueShownAsLabel = true;
            series1.LabelBackColor = System.Drawing.Color.White;
            series1.LabelBorderColor = System.Drawing.Color.Transparent;
            series1.Legend = "Frequency";
            series1.MarkerColor = System.Drawing.Color.Transparent;
            series1.Name = "Frequency";
            series1.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Red;
            series2.Legend = "Percent";
            series2.MarkerColor = System.Drawing.Color.Red;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "Percent";
            series2.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series2.YValuesPerPoint = 10;
            this.pareto.Series.Add(series1);
            this.pareto.Series.Add(series2);
            this.pareto.Size = new System.Drawing.Size(1983, 553);
            this.pareto.TabIndex = 0;
            this.pareto.Text = "chart1";
            // 
            // distribution
            // 
            chartArea2.Name = "ChartArea1";
            this.distribution.ChartAreas.Add(chartArea2);
            this.distribution.Location = new System.Drawing.Point(21, 598);
            this.distribution.Name = "distribution";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.Name = "Series1";
            this.distribution.Series.Add(series3);
            this.distribution.Size = new System.Drawing.Size(1983, 479);
            this.distribution.TabIndex = 1;
            this.distribution.Text = "chart2";
            // 
            // BeforeFiltered
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2041, 1089);
            this.Controls.Add(this.distribution);
            this.Controls.Add(this.pareto);
            this.Name = "BeforeFiltered";
            this.Text = "BeforeFiltered";
            ((System.ComponentModel.ISupportInitialize)(this.pareto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.distribution)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart pareto;
        private System.Windows.Forms.DataVisualization.Charting.Chart distribution;
    }
}