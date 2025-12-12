import { HttpClient } from '@angular/common/http';
import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { AuthResponse } from '../../models/auth-response';
import { AuthRequest } from '../../models/auth-request';
import { env } from '../../Environment/env';
import { Observable,tap } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private platformId = inject(PLATFORM_ID);

  constructor(private http:HttpClient,private router:Router){}

  login(credentials:AuthRequest):Observable<AuthResponse>{
    return this.http.post<AuthResponse>(`${env.apiBaseUrl}/auth/login`,credentials)
    .pipe(
      tap((response:AuthResponse)=>{
        this.SetJwtToken(response.jwtToken);
        this.SetOwinToken(response.owinToken);
        this.SetRefreshToken(response.refreshToken)
      })
    );
  }

   signup(credentials:AuthRequest):Observable<AuthResponse>{
    return this.http.post<AuthResponse>(`${env.apiBaseUrl}/auth/signup`,credentials)
    .pipe(
      tap((response:AuthResponse)=>{
        this.SetJwtToken(response.jwtToken);
        this.SetOwinToken(response.owinToken);
        this.SetRefreshToken(response.refreshToken)
      })
    );
  }

  refreshToken():Observable<AuthResponse>{
    return this.http.post<AuthResponse>(`${env.apiBaseUrl}/auth/refresh-token`,{
      refreshToken:this.getRefreshToken()
    })
    .pipe(
      tap((response:AuthResponse)=>{
        this.SetJwtToken(response.jwtToken);
        this.SetOwinToken(response.owinToken);
        this.SetRefreshToken(response.refreshToken)
      })
    );
  }

  logout(){
    if(isPlatformBrowser(this.platformId)){
      this.RemoveJwtToken();
      this.RemoveOwinToken();
      this.RemoveRefreshToken();
    }
    this.router.navigate(['/login']);
  }
  SetJwtToken(token:string):void{
    if(isPlatformBrowser(this.platformId))
     localStorage.setItem('jwtToken',token); 
  }

  SetOwinToken(token:string):void{
    if(isPlatformBrowser(this.platformId))
     localStorage.setItem('owinToken',token);
  }

  SetRefreshToken(token:string){
    if(isPlatformBrowser(this.platformId))
     localStorage.setItem('refreshToken',token);
  }

  RemoveJwtToken(){
    if(isPlatformBrowser(this.platformId))
     localStorage.removeItem('jwtToken');
  }

  RemoveOwinToken(){
    if(isPlatformBrowser(this.platformId))
     localStorage.removeItem('owinToken');
  }

  RemoveRefreshToken(){
    if(isPlatformBrowser(this.platformId))
     localStorage.removeItem('refreshToken');
  }

  isAuthenticated():boolean{
    return !!this.getJwtToken()
  }

  getJwtToken(){
    if(isPlatformBrowser(this.platformId))
     return localStorage.getItem('jwtToken');
    return null;
  }

  getOwinToken(){
    if(isPlatformBrowser(this.platformId))
     return localStorage.getItem('owinToken');
    return null;
  }

  getRefreshToken(){
    if(isPlatformBrowser(this.platformId))
      return localStorage.getItem('refreshToken');
    return null;
  }
}
