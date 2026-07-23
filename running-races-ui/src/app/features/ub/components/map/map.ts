import { Component, OnInit, OnDestroy } from '@angular/core';
import * as L from 'leaflet';
import { inject } from '@angular/core';
import { SectionService } from '../../services/section.service';
import { Section } from '../../models/ub.models';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { Router } from '@angular/router';

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

  private readonly shadowUrl = 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png';
  private readonly iconUrl = 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png';
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


  private initMap(): void {
   
    L.Marker.prototype.options.icon = L.icon({
      iconUrl: this.iconUrl,
      shadowUrl: this.shadowUrl,
      iconSize: [10, 16],
      iconAnchor: [5, 16]
    });

    this.map = L.map('map').setView([46.87, 17.73], 10);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);
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
      const startIcon = L.icon({
        iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-green.png',
        shadowUrl: this.shadowUrl,
        iconSize: [20, 33],
        iconAnchor: [10, 33]
      });
      const start = section.startWayPoint;
      const end = section.endWayPoint;
      if (start?.lat && start?.lng && end?.lat && end?.lng) {
        L.polyline(
          [[start.lat, start.lng], [end.lat, end.lng]],
          { color: '#078080', weight: 3 }
        ).addTo(this.map);

        L.marker([start.lat, start.lng], {
          title: start.name,
          icon: isStart ? startIcon : L.Marker.prototype.options.icon
        })
          .bindPopup(`<b>${start.name}</b><br>${section.name} (${section.distance} km)`)
          .on('dblclick', () => this.router.navigate(['/ub/waypoints'], { queryParams: { search: start.name } }))
          .addTo(this.map);
      }
    });
  }
}