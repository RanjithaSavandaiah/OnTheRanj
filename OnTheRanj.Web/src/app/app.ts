import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as AuthActions from './store/actions/auth.actions';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

/**
 * Root application component
 * Contains router outlet for navigation
 */
@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  title = 'OnTheRanj - Timesheet Management';

  constructor(private store: Store) {}

  ngOnInit() {
    this.store.dispatch(AuthActions.loadUser());
  }
}
