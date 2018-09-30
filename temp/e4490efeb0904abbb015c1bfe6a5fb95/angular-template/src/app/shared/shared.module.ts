import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CustomModalComponent } from '@app/shared/custom-modal/custom-modal.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule
  ],
  declarations: [
    CustomModalComponent
  ],
  exports: [
    CommonModule,
    FormsModule,
    CustomModalComponent
  ]
})
export class SharedModule { }
