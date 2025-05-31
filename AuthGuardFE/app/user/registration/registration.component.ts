import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { FirstKeyPipe } from '../../shared/pipes/first-key.pipe';
import { AuthService } from '../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule,CommonModule,FirstKeyPipe,RouterLink],
  templateUrl: './registration.component.html',
  styles: ``
})
export class RegistrationComponent implements OnInit
{
  // constructor(public formBuilder:FormBuilder){}
  formBuilder=inject(FormBuilder);
  service=inject(AuthService);
  router=inject(Router);
  toastr=inject(ToastrService);
  isSubmitted:Boolean=false;
  passwordVisible = false;

  ngOnInit(): void {
    if(this.service.isLoggedIn())
    {
      this.router.navigateByUrl('/dashboard');
    }
  }

  passwordMatchValidator:ValidatorFn=(control:AbstractControl):null=>
  {
    const password=control.get("password");
    const confirmPassword=control.get("confirmPassword");

    if(password && confirmPassword && password.value!=confirmPassword.value)
      confirmPassword?.setErrors({passwordMismatch:true})
    else 
      confirmPassword?.setErrors(null)
    return null;
  }

  form=this.formBuilder.group({
    fullName: ['',Validators.required],
    email:['',[Validators.required,Validators.email]],
    password:['',[Validators.required,Validators.minLength(6),Validators.pattern(/(?=.*[^a-zA-Z0-9])/)]],
    confirmPassword:['']
  },{validators:this.passwordMatchValidator})

  onSubmit() {
    this.isSubmitted=true;
    if(this.form.valid)
    {
      console.log(this.form.value);
      this.service.createUser(this.form.value)
      .subscribe({
        next:(res:any) =>{
          if(res.succeeded)
          {
            this.form.reset();
            this.isSubmitted=false;
            this.toastr.success("New User Created!","Registration Successfull")
          }
        },
        error:(err:any)=>{
          if(err.error.errors){
            err.error.errors.forEach((x:any) => 
            {
              switch(x.code)
              {
                case "DuplicateUser":
                  break;
                case "DuplicateEmail":
                  this.toastr.error("email is already Taken","Registration Failed")
                  break;
                default:
                  this.toastr.error("Contact the developer","Registration Failed")
                  console.log(x);
                  break;
              }
            })}
          else
          {
            console.log("Error:",err)
          }
        }
      });
    }
  }
  

  hasDisplayableError(controlName:string):Boolean
  {
    const control=this.form.get(controlName);
    return Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched))
  }

  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
  }

}
