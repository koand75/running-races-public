import { Component, OnInit, OnDestroy } from '@angular/core';
import * as L from 'leaflet';
import { inject } from '@angular/core';
import { SectionService } from '../../services/section.service';
import { Section } from '../../models/ub.models';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { Router } from '@angular/router';
import { MapService } from '../../services/map';

@Component({
  selector: 'app-map',
  standalone: true,
  imports: [MatIconModule, MatButtonModule, RouterLink],
  templateUrl: './map.html',
  styleUrl: './map.css'
})
export class MapComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private sectionService = inject(SectionService);
  private map!: L.Map;

  sections: Section[] = [];

  ngOnInit(): void {
    this.initMap();
    this.loadSections();
  }

  ngOnDestroy(): void {
    if (this.map) {
      this.map.remove();
    }
  }

  private mapService = inject(MapService);

  private initMap(): void {
    this.map = this.mapService.initMap('map', [46.87, 17.73], 10);
  }

  private loadSections(): void {
    this.sectionService.getAll().subscribe(sections => {
      this.sections = sections;
      this.drawSections();
    });
  }

  private drawSections(): void {
    this.sections.forEach(section => {
      const isStart = section.order === 1;
      const icon = isStart ? this.mapService.getStartIcon() : L.Marker.prototype.options.icon;
      const start = section.startWayPoint;
      const end = section.endWayPoint;
      if (start?.lat && start?.lng && end?.lat && end?.lng) {
        L.polyline(
          [[start.lat, start.lng], [end.lat, end.lng]],
          { color: '#078080', weight: 3 }
        ).addTo(this.map);

        this.mapService.createMarker(
          start.lat, start.lng, start.name,
          `<b>${start.name}</b><br>${section.name} (${section.distance} km)`,
          section.order === 1
        )
          .on('dblclick', () => this.router.navigate(['/ub/waypoints'], { queryParams: { search: start.name } }))
          .addTo(this.map);
      }
    });
  }
}