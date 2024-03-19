import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListProductComponent } from './Components/list-product/list-product.component';
import { AddProductComponent } from './Components/add-product/add-product.component';
import { EditProductComponent } from './Components/edit-product/edit-product.component';
import { AuthGuard } from './Auhentication/auth.guard';
import { LoginComponent } from './Auhentication/login/login.component';

const routes: Routes = [
  { path: "", redirectTo: "/ProductCategory", pathMatch: "full" },
  { path: "ProductCategory", component: ListProductComponent, pathMatch: "full", canActivate: [AuthGuard] },
  { path: "login", component: LoginComponent, },
  { path: "ProductCategory/create", component: AddProductComponent , canActivate: [AuthGuard] },
  /*The : id part is a placeholder for a dynamic segment that will be captured by the route.*/
  { path: "ProductCategory/:id/update", component: EditProductComponent },
  { path: "ProductCategory/edit/:id", component: EditProductComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
