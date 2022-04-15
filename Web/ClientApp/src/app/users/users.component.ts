import {Component, Inject} from "@angular/core";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-users-component',
  templateUrl: './users.component.html'
})
export class UsersComponent {
  public users: UserDto[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<UserDto[]>(baseUrl + 'users').subscribe(result => {
      this.users = result;
    }, error => console.error(error));
  }
}

interface UserDto {
  id: number;
  name: string;
  email: string
}
