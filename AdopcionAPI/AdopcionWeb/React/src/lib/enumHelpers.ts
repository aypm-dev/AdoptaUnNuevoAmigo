// Handles both numeric C# enum values (default Inertia serialization) and string enum values.

export type ResultadoVisitaLabel = "Pendiente" | "Favorable" | "Desfavorable";
export type EstadoAnimalLabel =
  | "Disponible"
  | "EnProceso"
  | "EnSeguimiento"
  | "Adoptado"
  | "NoAdoptable";
export type EstadoSolicitudLabel =
  | "Enviada"
  | "EnRevision"
  | "VisitaProgramada"
  | "Aprobada"
  | "Rechazada"
  | "Cancelada";
export type EstadoSeguimientoLabel =
  | "Pendiente"
  | "Favorable"
  | "Desfavorable"
  | "Vencido"
  | "Omitido";

const RESULTADO_VISITA_MAP: Record<string, ResultadoVisitaLabel> = {
  "0": "Pendiente",
  "1": "Favorable",
  "2": "Desfavorable",
  Pendiente: "Pendiente",
  Favorable: "Favorable",
  Desfavorable: "Desfavorable",
};

const ESTADO_SEGUIMIENTO_MAP: Record<string, EstadoSeguimientoLabel> = {
  "0": "Pendiente",
  "1": "Favorable",
  "2": "Desfavorable",
  "3": "Vencido",
  "4": "Omitido",
  Pendiente: "Pendiente",
  Favorable: "Favorable",
  Desfavorable: "Desfavorable",
  Vencido: "Vencido",
  Omitido: "Omitido",
};

const HITO_DAYS_MAP: Record<string, number> = {
  "30": 30,
  "60": 60,
  "90": 90,
  Dias30: 30,
  Dias60: 60,
  Dias90: 90,
};

const ESTADO_SOLICITUD_MAP: Record<string, EstadoSolicitudLabel> = {
  "0": "Enviada",
  "1": "EnRevision",
  "2": "VisitaProgramada",
  "3": "Aprobada",
  "4": "Rechazada",
  "5": "Cancelada",
  Enviada: "Enviada",
  EnRevision: "EnRevision",
  VisitaProgramada: "VisitaProgramada",
  Aprobada: "Aprobada",
  Rechazada: "Rechazada",
  Cancelada: "Cancelada",
};

const ESTADO_ANIMAL_MAP: Record<string, EstadoAnimalLabel> = {
  "0": "Disponible",
  "1": "EnProceso",
  "2": "EnSeguimiento",
  "3": "Adoptado",
  "4": "NoAdoptable",
  Disponible: "Disponible",
  EnProceso: "EnProceso",
  EnSeguimiento: "EnSeguimiento",
  Adoptado: "Adoptado",
  NoAdoptable: "NoAdoptable",
};

export function getEstadoAnimalLabel(value: number | string): EstadoAnimalLabel {
  return ESTADO_ANIMAL_MAP[String(value)] ?? "Disponible";
}

export function getEstadoSolicitudLabel(value: number | string): EstadoSolicitudLabel {
  return ESTADO_SOLICITUD_MAP[String(value)] ?? "Enviada";
}

export function getResultadoVisitaLabel(value: number | string): ResultadoVisitaLabel {
  return RESULTADO_VISITA_MAP[String(value)] ?? "Pendiente";
}

export function getEstadoSeguimientoLabel(value: number | string): EstadoSeguimientoLabel {
  return ESTADO_SEGUIMIENTO_MAP[String(value)] ?? "Pendiente";
}

export function getHitoDays(value: number | string): number {
  return HITO_DAYS_MAP[String(value)] ?? 30;
}
