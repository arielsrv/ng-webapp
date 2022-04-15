import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { UserDto } from "./userDto";

@Component({
  selector: 'app-users-component',
  templateUrl: './users.component.html'
})
export class UsersComponent {
  public users: UserDto[] = [];

  constructor(
    httpClient: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    httpClient.get<UserDto[]>(baseUrl + 'users')
      .subscribe(result => {
          this.users = result;
      }, error => console.error(error));
  }
}
