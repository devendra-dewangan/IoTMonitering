import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
  host: {
    class: 'fixed w-full bg-gray-800 text-white p-4'
  }
})
export class NavbarComponent {

}
