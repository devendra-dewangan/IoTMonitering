import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  host: {
    class: 'flex flex-col min-h-screen'
  }
})
export class AppComponent {
  title = 'IoTMoniteriing.Ui';
}
