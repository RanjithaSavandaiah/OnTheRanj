import { ChartType, ChartOptions } from 'chart.js';
import { NgChartsModule } from 'ng2-charts';
import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ToolbarComponent } from '../../../shared/layout/toolbar/toolbar.component';
import { MatIconModule } from '@angular/material/icon';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { ReportService } from '../../../core/services/report.service';
import { EmployeeHoursSummary, ProjectHoursSummary, BillableHoursSummary } from '../../../core/models/report.model';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';
import pdfMake from 'pdfmake/build/pdfmake';
import pdfFonts from 'pdfmake/build/vfs_fonts';
pdfMake.vfs = pdfFonts.vfs;

@Component({
  selector: 'app-reports',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatTabsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    ReactiveFormsModule,
    NgChartsModule,
    ToolbarComponent,
    ToolbarComponent
  ],
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent {
    // Sidenav links for MainLayoutComponent
    sidenavLinks = [
      { label: 'Dashboard', icon: 'dashboard', route: '/manager/dashboard' },
      { label: 'Projects', icon: 'work', route: '/manager/projects' },
      { label: 'Assignments', icon: 'assignment_ind', route: '/manager/assignments' },
      { label: 'Timesheet Approvals', icon: 'check_circle', route: '/manager/timesheet-approvals' },
      { label: 'Reports', icon: 'bar_chart', route: '/manager/reports' }
    ];
  auth = inject(AuthService);
  router = inject(Router);
  get currentUser() {
    return this.auth.currentUser();
  }

  onLogout() {
    this.auth.logout();
  }
  form: FormGroup;
  loading = false;
  activeTab = 0;

  employeeData: EmployeeHoursSummary[] = [];
  projectData: ProjectHoursSummary[] = [];
  billableData: BillableHoursSummary | null = null;

  displayedEmployeeColumns = ['employeeName', 'totalHours', 'billableHours', 'nonBillableHours'];
  displayedProjectColumns = ['projectCode', 'projectName', 'clientName', 'totalHours', 'employeeCount'];

  constructor(private fb: FormBuilder, private reportService: ReportService) {
    const today = new Date();
    const start = new Date(today.getFullYear(), today.getMonth(), 1);
    const end = new Date(today.getFullYear(), today.getMonth() + 1, 0);
    this.form = this.fb.group({
      startDate: [start],
      endDate: [end]
    });
    this.loadAllReports();
  }

    totalHours: number = 0;
    billableHours: number = 0;
    nonBillableHours: number = 0;
    topProjects: any[] = [];
    topEmployees: any[] = [];

    // Chart.js config
    public pieChartData = {
      labels: ['Billable', 'Non-Billable'],
      datasets: [
        {
          data: [0, 0],
          backgroundColor: ['#43a047', '#e53935']
        }
      ]
    };
    public pieChartType: ChartType = 'pie';
    public pieChartOptions: ChartOptions = {
      responsive: true,
      plugins: {
        legend: { position: 'bottom' },
        tooltip: { enabled: true }
      }
    };

    ngOnInit(): void {
      this.loadDashboardSummary();
    }

    loadDashboardSummary() {
      this.reportService.getDashboardSummary().subscribe(summary => {
        this.totalHours = summary.totalHours;
        this.billableHours = summary.billableHours;
        this.nonBillableHours = summary.nonBillableHours;
        this.topProjects = summary.topProjects;
        this.topEmployees = summary.topEmployees;
        this.pieChartData = {
          labels: ['Billable', 'Non-Billable'],
          datasets: [
            {
              data: [summary.billableHours, summary.nonBillableHours],
              backgroundColor: ['#43a047', '#e53935']
            }
          ]
        };
      });
    }

  loadAllReports() {
    const { startDate, endDate } = this.form.value;
    if (!startDate || !endDate) return;
    this.loading = true;
    this.reportService.getEmployeeHoursSummary(startDate, endDate)
      .pipe(finalize(() => this.loading = false))
      .subscribe(data => this.employeeData = data);
    this.reportService.getProjectHoursSummary(startDate, endDate)
      .subscribe(data => this.projectData = data);
    this.reportService.getBillableHoursSummary(startDate, endDate)
      .subscribe(data => this.billableData = data);
  }

  onTabChange(index: number) {
    this.activeTab = index;
  }

  onDateChange() {
    this.loadAllReports();
  }

  downloadEmployeeReportExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.employeeData);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Employee Summary');
    const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    saveAs(new Blob([excelBuffer], { type: 'application/octet-stream' }), 'employee-summary.xlsx');
  }

  downloadEmployeeReportPDF() {
    const docDefinition = {
      content: [
        { text: 'Employee Summary', style: 'header' },
        {
          table: {
            headerRows: 1,
            widths: ['*', '*', '*', '*'],
            body: [
              ['Employee', 'Total Hours', 'Billable', 'Non-Billable'],
              ...this.employeeData.map(row => [row.employeeName, row.totalHours, row.billableHours, row.nonBillableHours])
            ]
          }
        }
      ],
      styles: {
        header: { fontSize: 18, bold: true, margin: [0, 0, 0, 10] }
      }
    };
    pdfMake.createPdf(docDefinition).download('employee-summary.pdf');
  }

  downloadProjectReportExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(this.projectData);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Project Summary');
    const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    saveAs(new Blob([excelBuffer], { type: 'application/octet-stream' }), 'project-summary.xlsx');
  }

  downloadProjectReportPDF() {
    const docDefinition = {
      content: [
        { text: 'Project Summary', style: 'header' },
        {
          table: {
            headerRows: 1,
            widths: ['*', '*', '*', '*', '*'],
            body: [
              ['Code', 'Project', 'Client', 'Total Hours', 'Employees'],
              ...this.projectData.map(row => [row.projectCode, row.projectName, row.clientName, row.totalHours, row.employeeCount])
            ]
          }
        }
      ],
      styles: {
        header: { fontSize: 18, bold: true, margin: [0, 0, 0, 10] }
      }
    };
    pdfMake.createPdf(docDefinition).download('project-summary.pdf');
  }

  downloadBillableReportExcel() {
    if (!this.billableData) return;
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet([
      {
        'Billable Hours': this.billableData.totalBillableHours,
        'Non-Billable Hours': this.billableData.totalNonBillableHours,
        'Total Hours': this.billableData.totalHours,
        'Billable %': this.billableData.billablePercentage
      }
    ]);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Billable Summary');
    const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    saveAs(new Blob([excelBuffer], { type: 'application/octet-stream' }), 'billable-summary.xlsx');
  }

  downloadBillableReportPDF() {
    if (!this.billableData) return;
    const docDefinition = {
      content: [
        { text: 'Billable Summary', style: 'header' },
        {
          table: {
            headerRows: 1,
            widths: ['*', '*', '*', '*'],
            body: [
              ['Billable Hours', 'Non-Billable Hours', 'Total Hours', 'Billable %'],
              [
                this.billableData.totalBillableHours,
                this.billableData.totalNonBillableHours,
                this.billableData.totalHours,
                this.billableData.billablePercentage
              ]
            ]
          }
        }
      ],
      styles: {
        header: { fontSize: 18, bold: true, margin: [0, 0, 0, 10] }
      }
    };
    pdfMake.createPdf(docDefinition).download('billable-summary.pdf');
  }
}
