import { ActivatedRoute } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MyAssignmentsComponent } from './my-assignments.component';
import { provideMockStore, MockStore } from '@ngrx/store/testing';

import { HttpClientTestingModule } from '@angular/common/http/testing';

const initialState = {
  assignments: {
    entities: {},
    ids: [],
    loading: false,
    error: null
  },
  auth: {
    user: { id: 1, fullName: 'Test User', email: 'test@example.com', role: 'Employee' },
    loading: false,
    error: null
  }
};

describe('MyAssignmentsComponent', () => {
  let component: MyAssignmentsComponent;
  let fixture: ComponentFixture<MyAssignmentsComponent>;
  let store: MockStore;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyAssignmentsComponent, HttpClientTestingModule],
      providers: [
        provideMockStore({ initialState }),
        { provide: ActivatedRoute, useValue: {} }
      ]
    }).compileComponents();
    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(MyAssignmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display a message if there are no assignments', () => {
    const el = fixture.debugElement.nativeElement;
    expect(el.textContent.toLowerCase()).toContain('no assignments');
  });

  it('should render assignments table with correct data', () => {
    component.assignments = [
      { id: 1, employeeId: 1, projectCodeId: 1, projectName: 'Alpha', startDate: '2025-12-01', endDate: '2025-12-31' },
      { id: 2, employeeId: 1, projectCodeId: 2, projectName: 'Beta', startDate: '2025-11-01', endDate: null }
    ];
    fixture.detectChanges();
    const el = fixture.debugElement.nativeElement;
    expect(el.textContent).toContain('Alpha');
    expect(el.textContent).toContain('Beta');
    // Angular date pipe default: 'MMM d, y'
    expect(el.textContent).toContain('Dec 1, 2025');
    expect(el.textContent).toContain('Dec 31, 2025');
    expect(el.textContent).toContain('Nov 1, 2025');
    // Ongoing for null endDate
    expect(el.textContent).toContain('Ongoing');
  });

});
