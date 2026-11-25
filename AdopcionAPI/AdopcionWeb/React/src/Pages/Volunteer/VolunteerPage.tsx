import { useState } from "react";
import { Head, Link, router } from "@inertiajs/react";
import {
  Button,
  Card,
  CardBody,
  Chip,
  Navbar,
  NavbarBrand,
  NavbarContent,
  NavbarItem,
  Tab,
  Tabs,
} from "@heroui/react";
import { Footer } from "@/components/Footer";
import { PawIcon } from "@/components/PawIcon";
import { getEstadoSeguimientoLabel, getResultadoVisitaLabel } from "@/lib/enumHelpers";
import { FollowUpGroupCard } from "@/Pages/Volunteer/FollowUpGroupCard";
import { VisitCard } from "@/Pages/Volunteer/VisitCard";
import type {
  AdoptanteDto,
  AnimalDto,
  SeguimientoDto,
  SolicitudDto,
  VacunacionDto,
  VisitaDto,
} from "@/types/models";

interface VolunteerPageProps {
  voluntarioNombre: string;
  visitas: VisitaDto[];
  seguimientos: SeguimientoDto[];
  solicitudes: SolicitudDto[];
  animales: AnimalDto[];
  adoptantes: AdoptanteDto[];
  vacunaciones: VacunacionDto[];
  flashSuccess?: string | null;
  flashError?: string | null;
  apiError?: string | null;
}

export default function VolunteerPage({
  voluntarioNombre,
  visitas,
  seguimientos,
  solicitudes,
  animales,
  adoptantes,
  vacunaciones,
  flashSuccess,
  flashError,
  apiError,
}: VolunteerPageProps) {
  const [selectedTab, setSelectedTab] = useState<string>("visitas");

  const solicitudById = new Map(solicitudes.map((s) => [String(s.idSolicitud), s]));
  const animalById = new Map(animales.map((a) => [String(a.idAnimal), a]));
  const adoptanteById = new Map(adoptantes.map((a) => [String(a.idAdoptante), a]));

  const pendingVisits = visitas.filter(
    (v) => getResultadoVisitaLabel(v.resultado) === "Pendiente"
  );
  const completedVisits = visitas.filter(
    (v) => getResultadoVisitaLabel(v.resultado) !== "Pendiente"
  );

  // Group follow-ups by solicitudId so each adoption shows its 30 / 60 / 90 cycle together
  const followUpGroupsMap = new Map<number, SeguimientoDto[]>();
  for (const s of seguimientos) {
    const sid = Number(s.solicitudId);
    const list = followUpGroupsMap.get(sid) ?? [];
    list.push(s);
    followUpGroupsMap.set(sid, list);
  }

  const allFollowUpGroups = Array.from(followUpGroupsMap.entries()).map(
    ([solicitudId, items]) => ({
      solicitudId,
      items,
      hasOpen: items.some((i) => {
        const label = getEstadoSeguimientoLabel(i.estado);
        return label === "Pendiente" || label === "Vencido";
      }),
    })
  );

  const openFollowUpGroups = allFollowUpGroups.filter((g) => g.hasOpen);
  const closedFollowUpGroups = allFollowUpGroups.filter((g) => !g.hasOpen);

  return (
    <>
      <Head title="Panel de Voluntario" />
      <div className="flex min-h-screen flex-col bg-[#FAF8F5] text-[#11181C]">
        <Navbar
          maxWidth="xl"
          isBordered
          className="border-black/5 bg-[#FAF8F5]/90 backdrop-blur-md"
        >
          <NavbarBrand>
            <Link
              href="/"
              className="flex items-center gap-2.5 text-[#11181C] no-underline"
            >
              <PawIcon color="#292D32" size={22} />
              <span className="text-[15px] font-bold tracking-tight">
                Adopta Un Nuevo Amigo
              </span>
            </Link>
          </NavbarBrand>

          <NavbarContent justify="end" className="gap-3">
            <NavbarItem className="hidden sm:flex">
              <Button
                as={Link}
                href="/"
                size="sm"
                radius="full"
                variant="light"
                className="font-medium text-[#52525B]"
              >
                Ver catálogo público
              </Button>
            </NavbarItem>
            <NavbarItem>
              <Chip
                size="sm"
                variant="flat"
                className="bg-amber-100 font-semibold text-amber-800"
              >
                Voluntario
              </Chip>
            </NavbarItem>
          </NavbarContent>
        </Navbar>

        <main className="mx-auto w-full max-w-6xl flex-1 px-6 pb-8 pt-6 md:px-8 md:pt-8">
          {/* Greeting header matching Figma */}
          <section className="flex items-center justify-between gap-4 md:rounded-3xl md:border md:border-black/5 md:bg-white/75 md:p-7 md:shadow-sm">
            <div>
              <h1 className="text-3xl font-bold tracking-tight text-[#11181C] md:text-4xl">
                Bienvenido, {voluntarioNombre}.
              </h1>
              <p className="mt-1 text-sm text-[#52525B]">
                Visitas y seguimientos asignados
              </p>
            </div>
            <img
              src="/images/brand/hero-mascot.png"
              alt="Adopta Un Nuevo Amigo"
              className="h-16 w-20 shrink-0 object-contain md:h-20 md:w-24"
            />
          </section>

          {/* Status banners */}
          {apiError && (
            <Card className="mt-5 border border-danger-200 bg-danger-50 shadow-sm">
              <CardBody className="flex flex-row items-center justify-between gap-4">
                <div>
                  <p className="text-sm font-semibold text-danger-700">
                    No se pudo conectar con AdopcionAPI
                  </p>
                  <p className="text-xs text-danger-600">{apiError}</p>
                </div>
                <Button
                  size="sm"
                  color="danger"
                  variant="flat"
                  onPress={() => router.reload()}
                >
                  Reintentar
                </Button>
              </CardBody>
            </Card>
          )}

          {flashSuccess && (
            <div className="mt-5 rounded-2xl border border-success-200 bg-success-50 px-4 py-3 text-sm font-medium text-success-700">
              {flashSuccess}
            </div>
          )}

          {flashError && (
            <div className="mt-5 rounded-2xl border border-danger-200 bg-danger-50 px-4 py-3 text-sm font-medium text-danger-700">
              {flashError}
            </div>
          )}

          {/* 2 Tabs: Visitas pendientes & Seguimientos pendientes (3rd out-of-scope tab removed) */}
          <div className="mt-6">
            <Tabs
              selectedKey={selectedTab}
              onSelectionChange={(key) => setSelectedTab(String(key))}
              color="warning"
              variant="underlined"
              classNames={{
                tabList: "gap-6 border-b border-black/10 w-full",
                tab: "px-1 pb-3 font-semibold",
              }}
            >
              <Tab
                key="visitas"
                title={
                  <div className="flex items-center gap-2">
                    <span>Visitas pendientes</span>
                    <Chip size="sm" variant="flat" color="warning">
                      {pendingVisits.length}
                    </Chip>
                  </div>
                }
              >
                <section className="mt-5">
                  <h2 className="mb-4 text-lg font-bold text-[#11181C]">
                    Visitas domiciliarias pendientes
                  </h2>

                  {pendingVisits.length === 0 ? (
                    <Card className="border border-dashed border-black/15 bg-white/75 shadow-none">
                      <CardBody className="py-10 text-center">
                        <p className="text-sm font-semibold text-[#11181C]">
                          No tiene visitas pendientes en este momento
                        </p>
                        <p className="mt-1 text-xs text-[#52525B]">
                          Cuando un coordinador le asigne una nueva visita domiciliaria, aparecerá aquí.
                        </p>
                      </CardBody>
                    </Card>
                  ) : (
                    <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                      {pendingVisits.map((visita) => {
                        const sol = solicitudById.get(String(visita.solicitudId));
                        const animal = sol
                          ? animalById.get(String(sol.animalId))
                          : undefined;
                        const adoptante = sol
                          ? adoptanteById.get(String(sol.adoptanteId))
                          : undefined;

                        return (
                          <VisitCard
                            key={String(visita.idVisita)}
                            visita={visita}
                            animal={animal}
                            adoptante={adoptante}
                            vacunaciones={vacunaciones}
                          />
                        );
                      })}
                    </div>
                  )}

                  {completedVisits.length > 0 && (
                    <div className="mt-10">
                      <h3 className="mb-4 text-base font-bold text-[#52525B]">
                        Visitas registradas recientemente ({completedVisits.length})
                      </h3>
                      <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                        {completedVisits.map((visita) => {
                          const sol = solicitudById.get(String(visita.solicitudId));
                          const animal = sol
                            ? animalById.get(String(sol.animalId))
                            : undefined;
                          const adoptante = sol
                            ? adoptanteById.get(String(sol.adoptanteId))
                            : undefined;

                          return (
                            <VisitCard
                              key={String(visita.idVisita)}
                              visita={visita}
                              animal={animal}
                              adoptante={adoptante}
                              vacunaciones={vacunaciones}
                            />
                          );
                        })}
                      </div>
                    </div>
                  )}
                </section>
              </Tab>

              <Tab
                key="seguimientos"
                title={
                  <div className="flex items-center gap-2">
                    <span>Seguimientos pendientes</span>
                    <Chip size="sm" variant="flat" color="warning">
                      {openFollowUpGroups.length}
                    </Chip>
                  </div>
                }
              >
                <section className="mt-5">
                  <h2 className="mb-4 text-lg font-bold text-[#11181C]">
                    Animales a los que realiza seguimiento
                  </h2>

                  {openFollowUpGroups.length === 0 ? (
                    <Card className="border border-dashed border-black/15 bg-white/75 shadow-none">
                      <CardBody className="py-10 text-center">
                        <p className="text-sm font-semibold text-[#11181C]">
                          No hay seguimientos pendientes en este momento
                        </p>
                        <p className="mt-1 text-xs text-[#52525B]">
                          Los seguimientos de 30, 60 y 90 días se generan automáticamente al aprobar una adopción.
                        </p>
                      </CardBody>
                    </Card>
                  ) : (
                    <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                      {openFollowUpGroups.map((group) => {
                        const sol = solicitudById.get(String(group.solicitudId));
                        const animal = sol
                          ? animalById.get(String(sol.animalId))
                          : undefined;
                        const adoptante = sol
                          ? adoptanteById.get(String(sol.adoptanteId))
                          : undefined;

                        return (
                          <FollowUpGroupCard
                            key={group.solicitudId}
                            solicitudId={group.solicitudId}
                            seguimientos={group.items}
                            animal={animal}
                            adoptante={adoptante}
                          />
                        );
                      })}
                    </div>
                  )}

                  {closedFollowUpGroups.length > 0 && (
                    <div className="mt-10">
                      <h3 className="mb-4 text-base font-bold text-[#52525B]">
                        Ciclos de seguimiento cerrados ({closedFollowUpGroups.length})
                      </h3>
                      <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                        {closedFollowUpGroups.map((group) => {
                          const sol = solicitudById.get(String(group.solicitudId));
                          const animal = sol
                            ? animalById.get(String(sol.animalId))
                            : undefined;
                          const adoptante = sol
                            ? adoptanteById.get(String(sol.adoptanteId))
                            : undefined;

                          return (
                            <FollowUpGroupCard
                              key={group.solicitudId}
                              solicitudId={group.solicitudId}
                              seguimientos={group.items}
                              animal={animal}
                              adoptante={adoptante}
                            />
                          );
                        })}
                      </div>
                    </div>
                  )}
                </section>
              </Tab>
            </Tabs>
          </div>
        </main>

        <Footer />
      </div>
    </>
  );
}
