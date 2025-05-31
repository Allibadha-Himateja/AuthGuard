import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN } from '../constants';
import { environment } from '../../../environments/environment.development';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }
  

  createUser(formData:any)
  {
    return this.http.post(environment.apiBaseUrl+'/signup',formData)
  }

  signIn(formData:any)
  {
    return this.http.post(environment.apiBaseUrl+'/signin',formData)
  }

  isLoggedIn()
  {
    return localStorage.getItem(TOKEN)!=null?true:false;
  }

  deletetoken()
  {
     localStorage.removeItem(TOKEN);
  }

  savetoken(res:any)
  {
    localStorage.setItem(TOKEN,res);
  }

  getToken(){
    return localStorage.getItem(TOKEN);
  }

  getClaims()
  {
    return JSON.parse(window.atob(this.getToken()!.split('.')[1]));
  }
}
