import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { CoreRoutingModule } from './core-routing-module';
import { Navbar } from './components/navbar/navbar';


@NgModule({
  declarations: [
    Navbar
  ],
  imports: [
    CommonModule,
    RouterModule,
    CoreRoutingModule
  ],
  exports: [
    Navbar
  ]
})
export class CoreModule { }
