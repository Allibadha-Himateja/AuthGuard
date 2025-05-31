import { CommonModule } from '@angular/common';
import { Component, inject, Inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { TOKEN } from '../../shared/constants';

@Component({
  selector: 'app-login',
  imports: [CommonModule,ReactiveFormsModule,RouterLink],
  templateUrl: './login.component.html',
  styles: ``
})
export class LoginComponent implements OnInit 
{
  
  formBuilder=inject(FormBuilder);
  service=inject(AuthService);
  router=inject(Router);
  toastr=inject(ToastrService);
  isSubmitted:Boolean=false;

  ngOnInit(): void {
    if(this.service.isLoggedIn())
    {
      this.router.navigateByUrl('/dashboard');
    }
  }
  
  form=this.formBuilder.group({
    email:['',[Validators.required,Validators.email]],
    password:['',Validators.required]
  });

  hasDisplayableError(controlName:string):Boolean
  {
    const control=this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched))
  }

  onSubmit()
  {
    this.isSubmitted=true;
    // console.log(this.form.value);
    this.service.signIn(this.form.value).subscribe(
      {
        next:(res:any)=>{
          // we have to store the token in the localStorage
          // console.log(res.token);
          this.service.savetoken(res.token);
          // we have to redirect to the dashboard
          // we can able to do that with the Router
          this.router.navigateByUrl("/dashboard");
        },
        error:(err:any)=>
        {
          if(err.status==400)
          {
            this.toastr.error("Incorrect Email or Password","Login Failed");
          }
          else{
            console.log("Error during login");
          }
        }
      }
    )
  }
}
