import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ToolbarComponent } from './toolbar.component';
describe('ToolbarComponent', () => {
  let component: ToolbarComponent;
  let fixture: ComponentFixture<ToolbarComponent>;


  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ToolbarComponent, NoopAnimationsModule],
    }).compileComponents();
    fixture = TestBed.createComponent(ToolbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });


  it('should render a mat-toolbar', () => {
    const toolbarDe = fixture.debugElement.query(By.css('mat-toolbar'));
    expect(toolbarDe).toBeTruthy();
  });


  it('should display the user full name if provided', () => {
    component.userFullName = 'Jane Doe';
    fixture.detectChanges();
    const toolbarEl: HTMLElement = fixture.nativeElement.querySelector('mat-toolbar');
    expect(toolbarEl.textContent).toContain('Jane Doe');
  });


  it('should emit logout event when logout button is clicked', () => {
    let called = false;
    component.logout.subscribe(() => { called = true; });
    const button = fixture.debugElement.query(By.css('button'));
    expect(button).toBeTruthy();
    button.nativeElement.click();
    expect(called).toBe(true);
  });

});
