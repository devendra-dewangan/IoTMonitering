import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './navbar/navbar.component';
import { Page404Component } from './page404/page404.component';
import { CopyrightsComponent } from './copyrights/copyrights.component';
import { AppRoutingModule } from "../app-routing.module";
import { ChartComponent } from './chart/chart.component';
import { BaseChartDirective } from 'ng2-charts';

@NgModule({
  declarations: [
    NavbarComponent,
    Page404Component,
    CopyrightsComponent,
    ChartComponent,
  ],
  imports: [
    CommonModule,
    BaseChartDirective
],
  exports: [
    NavbarComponent,
    CopyrightsComponent,
    ChartComponent
  ]

})
export class SharedModule { }
