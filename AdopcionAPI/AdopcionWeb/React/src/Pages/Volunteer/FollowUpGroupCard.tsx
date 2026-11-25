import { useState } from "react";
import { router } from "@inertiajs/react";
import { Button, Card, CardBody, Chip, Progress, Textarea } from "@heroui/react";
import { ConfirmModal } from "@/components/ConfirmModal";
import { PawIcon } from "@/components/PawIcon";
import { getEstadoSeguimientoLabel, getHitoDays } from "@/lib/enumHelpers";
import { formatDate, getPetImageUrl } from "@/lib/petHelpers";
import type {
  AdoptanteDto,
  AnimalDto,
  SeguimientoDto,
} from "@/types/models";

interface FollowUpGroupCardProps {
  solicitudId: number;
  seguimientos: SeguimientoDto[];
  animal?: AnimalDto;
  adoptante?: AdoptanteDto;
}

export function FollowUpGroupCard({
  solicitudId,
  seguimientos,
  animal,
  adoptante,
}: FollowUpGroupCardProps) {
  const [observaciones, setObservaciones] = useState("");
  const [localError, setLocalError] = useState<string | null>(null);
  const [pendingAction, setPendingAction] = useState<{
    seguimiento: SeguimientoDto;
    favorable: boolean;
  } | null>(null);
  const [submitting, setSubmitting] = useState(false);

  const sorted = [...seguimientos].sort(
    (a, b) => getHitoDays(a.hito) - getHitoDays(b.hito)
  );

  const first = sorted[0];
  const petName = animal?.nombre ?? first?.animalNombre ?? `Solicitud #${solicitudId}`;
  const adopterName = adoptante?.nombre ?? first?.adoptanteNombre ?? "Adoptante";
  const imageUrl = animal
    ? getPetImageUrl(animal)
    : `https://placehold.co/600x400/EDE8DF/52525B?text=${encodeURIComponent(petName)}`;

  const completedDays = sorted.reduce((max, s) => {
    if (getEstadoSeguimientoLabel(s.estado) === "Favorable") {
      return Math.max(max, getHitoDays(s.hito));
    }
    return max;
  }, 0);

  const activeMilestone = sorted.find((s) => {
    const label = getEstadoSeguimientoLabel(s.estado);
    return label === "Pendiente" || label === "Vencido";
  });
  const isFinalMilestone =
    activeMilestone !== undefined && getHitoDays(activeMilestone.hito) === 90;

  const requestAction = (seguimiento: SeguimientoDto, favorable: boolean) => {
    setLocalError(null);
    if (!favorable && !observaciones.trim()) {
      setLocalError("Las observaciones son obligatorias cuando se reporta una falla.");
      return;
    }
    setPendingAction({ seguimiento, favorable });
  };

  const confirmAction = () => {
    if (!pendingAction) return;
    setSubmitting(true);
    router.post(
      `/voluntario/seguimientos/${pendingAction.seguimiento.idSeguimiento}/resultado`,
      {
        favorable: pendingAction.favorable,
        observaciones: observaciones.trim() || null,
      },
      {
        preserveScroll: true,
        onFinish: () => {
          setSubmitting(false);
          setPendingAction(null);
          setObservaciones("");
        },
      }
    );
  };

  return (
    <>
      <Card className="overflow-hidden rounded-[20px] border border-black/5 bg-white shadow-[0_10px_24px_-6px_rgba(0,0,0,0.12)]">
        <CardBody className="gap-4 p-5">
          <div>
            {adoptante && (
              <p className="text-xs text-[#52525B]">
                Contacto: {adoptante.contacto}
              </p>
            )}
            <h3 className="mt-1 text-2xl font-bold tracking-tight text-[#11181C]">
              {petName} con {adopterName}
            </h3>
          </div>

          <div className="overflow-hidden rounded-2xl bg-stone-200">
            <img
              src={imageUrl}
              alt={petName}
              loading="lazy"
              className="h-48 w-full object-cover"
            />
          </div>

          {/* Progress bar matching Figma "Progreso del ciclo (60 / 90 días)" */}
          <div className="flex flex-col gap-1.5">
            <div className="flex items-center justify-between text-xs">
              <span className="font-semibold text-[#11181C]">
                Progreso del ciclo ({completedDays} / 90 días)
              </span>
              <span className="text-[#52525B]">
                {Math.round((completedDays / 90) * 100)}%
              </span>
            </div>
            <Progress
              aria-label="Progreso del seguimiento"
              value={(completedDays / 90) * 100}
              color="success"
              size="sm"
            />
          </div>

          {/* 30 / 60 / 90 milestones list */}
          <div className="flex flex-col gap-2.5">
            {sorted.map((s) => {
              const days = getHitoDays(s.hito);
              const estadoLabel = getEstadoSeguimientoLabel(s.estado);
              const isDone = estadoLabel === "Favorable";
              const isOverdue = estadoLabel === "Vencido";
              const isFailed = estadoLabel === "Desfavorable";

              return (
                <div
                  key={String(s.idSeguimiento)}
                  className="rounded-xl border border-black/5 bg-[#FAF8F5] px-3.5 py-2.5"
                >
                  <div className="flex items-center justify-between gap-2">
                    <span className="text-xs font-semibold text-[#11181C]">
                      Día {days} ·{" "}
                      {isDone && s.fechaRealizada
                        ? `Realizado: ${formatDate(s.fechaRealizada)}`
                        : `Programado: ${formatDate(s.fechaProgramada)}`}
                    </span>
                    <Chip
                      size="sm"
                      variant="flat"
                      color={
                        isDone
                          ? "success"
                          : isOverdue || isFailed
                            ? "danger"
                            : "warning"
                      }
                    >
                      {isDone ? "Completado" : estadoLabel}
                    </Chip>
                  </div>
                  {s.observaciones && (
                    <p className="mt-1 text-xs text-[#52525B]">{s.observaciones}</p>
                  )}
                </div>
              );
            })}
          </div>

          {/* Active milestone action form */}
          {activeMilestone && (
            <div className="flex flex-col gap-3 border-t border-black/5 pt-3">
              {localError && (
                <p className="rounded-xl bg-danger-50 px-3 py-2 text-xs font-medium text-danger-700">
                  {localError}
                </p>
              )}

              <Textarea
                label={`Observaciones (Día ${getHitoDays(String(activeMilestone.hito))})`}
                placeholder="Describa cómo se encuentra la mascota en su hogar..."
                value={observaciones}
                onValueChange={(v) => {
                  setObservaciones(v);
                  setLocalError(null);
                }}
                variant="flat"
                radius="lg"
                minRows={2}
              />

              <div className="flex flex-col gap-2">
                <Button
                  radius="full"
                  variant="flat"
                  color="success"
                  onPress={() => requestAction(activeMilestone, true)}
                  className="w-full font-semibold"
                  endContent={<PawIcon color="currentColor" size={14} />}
                >
                  {isFinalMilestone
                    ? "Registrar Hito Final (Adopción Definitiva)"
                    : `Registrar Seguimiento Día ${getHitoDays(String(activeMilestone.hito))}`}
                </Button>
                <Button
                  radius="full"
                  variant="flat"
                  color="danger"
                  onPress={() => requestAction(activeMilestone, false)}
                  className="w-full font-semibold"
                  endContent={<PawIcon color="currentColor" size={14} />}
                >
                  {isFinalMilestone
                    ? "Reportar Falla y Revertir a Disponible"
                    : "Reportar Falla en Seguimiento"}
                </Button>
              </div>
            </div>
          )}
        </CardBody>
      </Card>

      <ConfirmModal
        isOpen={pendingAction !== null}
        onClose={() => setPendingAction(null)}
        onConfirm={confirmAction}
        isLoading={submitting}
        title={
          pendingAction?.favorable
            ? isFinalMilestone
              ? "Confirmar adopción definitiva?"
              : "Registrar seguimiento favorable?"
            : isFinalMilestone
              ? "Revertir mascota a Disponible?"
              : "Reportar falla en seguimiento?"
        }
        subtitle={`${petName} con ${adopterName}`}
        description={
          pendingAction?.favorable
            ? isFinalMilestone
              ? `Al completar favorablemente el hito de 90 días, ${petName} pasará a estado Adoptado definitivo.`
              : "El seguimiento quedará marcado como completado con las observaciones ingresadas."
            : isFinalMilestone
              ? `Al reportar una falla en el seguimiento final de 90 días, ${petName} volverá al catálogo como Disponible y quedará registrado el evento.`
              : "El seguimiento quedará registrado como desfavorable para atención del coordinador."
        }
        question="Desea confirmar esta acción?"
        confirmText="Sí, confirmar"
        cancelText="Cancelar"
      />
    </>
  );
}
