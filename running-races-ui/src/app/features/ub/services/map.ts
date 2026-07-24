import { Injectable } from '@angular/core';
import * as L from 'leaflet';

@Injectable({
  providedIn: 'root'
})
export class MapService {
  private readonly iconUrl = 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png';
  private readonly shadowUrl = 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png';

  initMap(elementId: string, center: [number, number], zoom: number): L.Map {
    const map = L.map(elementId).setView(center, zoom);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(map);
    this.setDefaultIcon(map);
    return map;
  }

  createMarker(lat: number, lng: number, name: string, popupContent: string, isStart: boolean): L.Marker {
    return L.marker([lat, lng], {
      title: name,
      icon: isStart ? this.getStartIcon() : L.Marker.prototype.options.icon as L.Icon
    }).bindPopup(popupContent);
  }

  setDefaultIcon(map: L.Map): void {
    L.Marker.prototype.options.icon = L.icon({
      iconUrl: this.iconUrl,
      shadowUrl: this.shadowUrl,
      iconSize: [10, 16],
      iconAnchor: [5, 16]
    });
  }

  getStartIcon(): L.Icon {
    return L.icon({
      iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-green.png',
      shadowUrl: this.shadowUrl,
      iconSize: [20, 33],
      iconAnchor: [10, 33]
    });
  }

  destroyMap(map: L.Map): void {
    map.remove();
  }
}