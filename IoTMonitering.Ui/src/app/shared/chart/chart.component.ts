import { Component } from '@angular/core';
import { ChartOptions } from 'chart.js';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrl: './chart.component.css'
})
export class ChartComponent {
  chartData = [
    { data: [10, 20, 30, 25, 40], label: 'Telemetry' }
  ];

  chartLabels = ['1', '2', '3', '4', '5'];

  chartOptions:ChartOptions<'line'> = {
  responsive: false,
  layout: {
    padding: {
      left: 40,
      right: 40
    }
  },
  plugins: {
    legend: {
      labels: {
        font: {
          size: 25,
          family: 'Helvetica',
          weight: 'bold'
        }
      }
    }
  }
};
}
