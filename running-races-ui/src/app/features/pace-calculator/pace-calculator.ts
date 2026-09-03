import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-pace-calculator',
  standalone: true,
  imports: [FormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule, CommonModule, MatIcon],
  templateUrl: './pace-calculator.html',
  styleUrl: './pace-calculator.scss'
})
export class PaceCalculator {
  timeHours: number | null = null;
  timeMinutes: number | null = null;
  timeSeconds: number | null = null;

  distance: number | null = null;
  distanceUnit: string = 'km';
  distanceOptions = [
    { label: 'km', value: 1 },
    { label: 'm', value: 0.001 },
    { label: 'Marathon', value: 42.195 },
    { label: 'Félmaraton', value: 21.0975 },
    { label: '10k', value: 10 },
    { label: '5k', value: 5 }
  ];

  paceMinutes: number | null = null;
  paceSeconds: number | null = null;

  isTimeSet(): boolean {
    return this.timeHours !== null || this.timeMinutes !== null || this.timeSeconds !== null;
  }

  isDistanceSet(): boolean {
    return this.distance !== null;
  }

  isPaceSet(): boolean {
    return this.paceMinutes !== null || this.paceSeconds !== null;
  }

  filledCount(): number {
    return [this.isTimeSet(), this.isDistanceSet(), this.isPaceSet()].filter(Boolean).length;
  }

  canCalculate(): boolean {
    return this.filledCount() === 2;
  }

  getSelectedDistanceKm(): number {
    const opt = this.distanceOptions.find(o => o.label === this.distanceUnit);
    return (this.distance ?? 0) * (opt?.value ?? 1);
  }

  calculate(): void {
    const totalTimeSec = ((this.timeHours ?? 0) * 3600) + ((this.timeMinutes ?? 0) * 60) + (this.timeSeconds ?? 0);
    const distKm = this.getSelectedDistanceKm();
    const paceTotalSec = ((this.paceMinutes ?? 0) * 60) + (this.paceSeconds ?? 0);

    if (!this.isTimeSet()) {
      const calcTimeSec = paceTotalSec * distKm;
      this.timeHours = Math.floor(calcTimeSec / 3600);
      this.timeMinutes = Math.floor((calcTimeSec % 3600) / 60);
      this.timeSeconds = Math.round(calcTimeSec % 60);
    } else if (!this.isDistanceSet()) {
      const distanceKm = totalTimeSec / paceTotalSec;
      const opt = this.distanceOptions.find(o => o.label === this.distanceUnit);
      this.distance = distanceKm / (opt?.value ?? 1);
    } else {
      const calcPaceSec = totalTimeSec / distKm;
      this.paceMinutes = Math.floor(calcPaceSec / 60);
      this.paceSeconds = Math.round(calcPaceSec % 60);
    }
  }

  clearTime(): void { this.timeHours = null; this.timeMinutes = null; this.timeSeconds = null; }
  clearDistance(): void { this.distance = null; }
  clearPace(): void { this.paceMinutes = null; this.paceSeconds = null; }

  reset(): void {
    this.clearTime();
    this.clearDistance();
    this.clearPace();
    this.distanceUnit = 'km';
  }
}