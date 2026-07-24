import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import * as L from 'leaflet';
import { MapService } from '../../services/map';
import { SectionService } from '../../services/section.service';
import { RunnerSectionService } from '../../services/runner-section.service';
import { RunnerService } from '../../services/runner.service';
import { Section, Runner, RunnerSection } from '../../models/ub.models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-team-map',
  standalone: true,
  imports: [MatIconModule, MatButtonModule, RouterLink],
  templateUrl: './team-map.html',
  styleUrl: './team-map.css'
})
export class TeamMapComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private mapService = inject(MapService);
  private sectionService = inject(SectionService);
  private runnerService = inject(RunnerService);
  private runnerSectionService = inject(RunnerSectionService);

  private map!: L.Map;
  teamId!: number;
  sections: Section[] = [];
  runners: Runner[] = [];
  assignments: Map<number, RunnerSection> = new Map();

  readonly colors = [
    '#078080', '#f45d48', '#6c5ce7', '#00b894',
    '#fdcb6e', '#e17055', '#0984e3', '#b2bec3',
    '#d63031', '#00cec9', '#e84393', '#2d3436', '#fab1a0'
  ];

  ngOnInit(): void {
    this.teamId = Number(this.route.snapshot.paramMap.get('id'));
    this.map = this.mapService.initMap('team-map', [46.87, 17.73], 10);
    this.loadData();
  }

  ngOnDestroy(): void {
    this.mapService.destroyMap(this.map);
  }

  private loadData(): void {
    this.sectionService.getAll().subscribe(sections => {
      this.sections = sections;
      this.runnerService.getByTeam(this.teamId).subscribe(runners => {
        this.runners = runners;
        this.runnerSectionService.getByTeam(this.teamId).subscribe(assignments => {
          assignments.forEach(a => this.assignments.set(a.sectionId, a));
          this.drawMap();
        });
      });
    });
  }

  private drawMap(): void {
    this.sections.forEach(section => {
      const start = section.startWayPoint;
      const end = section.endWayPoint;
      if (!start?.lat || !start?.lng || !end?.lat || !end?.lng) return;

      const assignment = this.assignments.get(section.id);
      const runnerIndex = assignment
        ? this.runners.findIndex(r => r.id === assignment.runnerId)
        : -1;
      const color = runnerIndex >= 0 ? this.colors[runnerIndex % this.colors.length] : '#ccc';
      const runnerName = runnerIndex >= 0 ? this.runners[runnerIndex].name : 'Nincs futó';

      L.polyline(
        [[start.lat, start.lng], [end.lat, end.lng]],
        { color, weight: 8 }
      )
        .bindPopup(`${section.name}<br><b>${runnerName}</b>`)
        .addTo(this.map);;

      this.mapService.createMarker(
        start.lat, start.lng, start.name,
        `<b>${start.name}</b><br>${section.name} (${section.distance} km)`,
        section.order === 1
      )
        .on('dblclick', () => this.router.navigate(['/ub/waypoints'], { queryParams: { search: start.name } }))
        .addTo(this.map);

    });
  }
}