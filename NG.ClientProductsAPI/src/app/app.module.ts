import { HttpClientModule, withInterceptors } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ListProductComponent } from './Components/list-product/list-product.component';
import { AddProductComponent } from './Components/add-product/add-product.component';
import { EditProductComponent } from './Components/edit-product/edit-product.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { ToastrModule, ToastrService } from 'ngx-toastr';

import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient } from '@angular/common/http';
import { authInterceptor } from './Auhentication/auth.interceptor';
import {
  AggregateService, ColumnChooserService, ColumnMenuService, EditService, FilterService, GridModule, GroupService, PageService, PagerModule, ReorderService, ResizeService, SortService, SearchService,
  SelectionService,  ExcelExportService, PdfExportService, DetailRowService, ToolbarService } from '@syncfusion/ej2-angular-grids';
import { ChartModule } from '@syncfusion/ej2-angular-charts';
import { TextBoxModule, UploaderModule } from '@syncfusion/ej2-angular-inputs';
@NgModule({
  declarations: [
    AppComponent,
    ListProductComponent,
    AddProductComponent,
    EditProductComponent

  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule, CommonModule, ReactiveFormsModule,
    GridModule, PagerModule, ChartModule, TextBoxModule, UploaderModule,
    ToastrModule.forRoot() 
  ],
  providers: [

    PageService, SortService, FilterService, GroupService, EditService, AggregateService,
    ColumnChooserService, ColumnMenuService, ResizeService, ReorderService, SearchService,
    SelectionService, ExcelExportService, PdfExportService, DetailRowService, ToolbarService, 
    provideAnimations(),
    provideHttpClient(withInterceptors([authInterceptor])),
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
